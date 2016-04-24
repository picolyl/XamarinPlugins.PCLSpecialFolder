using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

//-----------------------------------------------------------------------
// This file is a derivation of:
// https://github.com/dsplaisted/PCLStorage/blob/master/src/PCLStorage.WinRT/WinRTFolder.cs
// Which is released under the Ms-PL license.
//-----------------------------------------------------------------------

namespace Plugin.PCLSpecialFolder
{
    /// <summary>
    /// Represents a folder in the <see cref="WinRTFileSystem"/>
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class SFWinRTFolder : IFolder
    {
        private readonly IStorageFolder _wrappedFolder;
        private readonly bool _isRootFolder;

        internal const int FILE_ALREADY_EXISTS = unchecked((int)0x800700B7);

        /// <summary>
        /// Creates a new <see cref="SFWinRTFolder"/>
        /// </summary>
        /// <param name="wrappedFolder">The WinRT <see cref="IStorageFolder"/> to wrap</param>
        public SFWinRTFolder(IStorageFolder wrappedFolder)
        {
            _wrappedFolder = wrappedFolder;
            if (_wrappedFolder.Path == string.Empty ||
                _wrappedFolder.Path == ApplicationData.Current.LocalFolder.Path ||
                _wrappedFolder.Path == ApplicationData.Current.RoamingFolder.Path ||
                _wrappedFolder.Path == ApplicationData.Current.TemporaryFolder.Path ||
#if WINDOWS_PHONE_APP || WINDOWS_UWP
                _wrappedFolder.Path == ApplicationData.Current.LocalCacheFolder.Path ||
#endif
                _wrappedFolder.Path == Windows.ApplicationModel.Package.Current.InstalledLocation.Path)
            {
                _isRootFolder = true;
            }
            else
            {
                _isRootFolder = false;
            }
        }

        /// <summary>
        /// Creates a new <see cref="SFWinRTFolder"/>
        /// </summary>
        /// <param name="wrappedFolder">The WinRT <see cref="IStorageFolder"/> to wrap</param>
        /// <param name="isRootFolder">The root folder flag</param>
        public SFWinRTFolder(IStorageFolder wrappedFolder, bool isRootFolder)
        {
            _wrappedFolder = wrappedFolder;
            _isRootFolder = isRootFolder;
        }

        /// <summary>
        /// The name of the folder
        /// </summary>
        public string Name
        {
            get { return _wrappedFolder.Name; }
        }

        /// <summary>
        /// The "full path" of the folder, which should uniquely identify it within a given <see cref="IFileSystem"/>
        /// </summary>
        public string Path
        {
            get { return _wrappedFolder.Path; }
        }

        /// <summary>
        /// Creates a file in this folder
        /// </summary>
        /// <param name="desiredName">The name of the file to create</param>
        /// <param name="option">Specifies how to behave if the specified file already exists</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created file</returns>
        public async Task<IFile> CreateFileAsync(string desiredName, PCLStorage.CreationCollisionOption option, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(desiredName, "desiredName");
            StorageFile wrtFile;
            try
            {
                wrtFile = await _wrappedFolder.CreateFileAsync(desiredName, GetWinRTCreationCollisionOption(option)).AsTask(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex.HResult == FILE_ALREADY_EXISTS)
                {
                    //  File already exists (and potentially other failures, not sure what the HResult represents)
                    throw new IOException(ex.Message, ex);
                }
                throw;
            }
            return new WinRTFile(wrtFile);
        }

        /// <summary>
        /// Gets a file in this folder
        /// </summary>
        /// <param name="name">The name of the file to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested file, or null if it does not exist</returns>
        public async Task<IFile> GetFileAsync(string name, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(name, "name");

            try
            {
                var wrtFile = await _wrappedFolder.GetFileAsync(name).AsTask(cancellationToken).ConfigureAwait(false);
                return new WinRTFile(wrtFile);
            }
            catch (FileNotFoundException ex)
            {
                throw new PCLStorage.Exceptions.FileNotFoundException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets a file in this folder
        /// </summary>
        /// <param name="name">The name of the file to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IFile> TryGetFileAsync(string name, CancellationToken cancellationToken)
        {
            IFile file = null;
            if (!cancellationToken.IsCancellationRequested)
            {
                var storageFile = await TryGetItemAsync(name).ConfigureAwait(false) as IStorageFile;
                if (storageFile != null)
                {
                    file = new WinRTFile(storageFile);
                }
            }
            return file;
        }

        /// <summary>
        /// Gets a list of the files in this folder
        /// </summary>
        /// <returns>A list of the files in the folder</returns>
        public async Task<IList<IFile>> GetFilesAsync(CancellationToken cancellationToken)
        {
            var wrtFiles = await _wrappedFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
            var files = wrtFiles.Select(f => new WinRTFile(f)).ToList<IFile>();
            return new ReadOnlyCollection<IFile>(files);
        }

        /// <summary>
        /// Creates a subfolder in this folder
        /// </summary>
        /// <param name="desiredName">The name of the folder to create</param>
        /// <param name="option">Specifies how to behave if the specified folder already exists</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created folder</returns>
        public async Task<IFolder> CreateFolderAsync(string desiredName, PCLStorage.CreationCollisionOption option, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(desiredName, "desiredName");
            StorageFolder wrtFolder;
            try
            {
                wrtFolder = await _wrappedFolder.CreateFolderAsync(desiredName, GetWinRTCreationCollisionOption(option)).AsTask(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex.HResult == FILE_ALREADY_EXISTS)
                {
                    //  Folder already exists (and potentially other failures, not sure what the HResult represents)
                    throw new IOException(ex.Message, ex);
                }
                throw;
            }
            return new SFWinRTFolder(wrtFolder);
        }

        /// <summary>
        /// Gets a subfolder in this folder
        /// </summary>
        /// <param name="name">The name of the folder to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested folder, or null if it does not exist</returns>
        public async Task<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(name, "name");

            StorageFolder wrtFolder;
            try
            {
                wrtFolder = await _wrappedFolder.GetFolderAsync(name).AsTask(cancellationToken).ConfigureAwait(false);
            }
            catch (FileNotFoundException ex)
            {
                //  Folder does not exist
                throw new PCLStorage.Exceptions.DirectoryNotFoundException(ex.Message, ex);
            }
            return new SFWinRTFolder(wrtFolder);
        }

        /// <summary>
        /// Gets a subfolder in this folder
        /// </summary>
        /// <param name="name">The name of the folder to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested folder, or null if it does not exist</returns>
        public async Task<IFolder> TryGetFolderAsync(string name, CancellationToken cancellationToken)
        {
            IFolder folder = null;
            if (!cancellationToken.IsCancellationRequested)
            {
                var storageFolder = await TryGetItemAsync(name).ConfigureAwait(false) as IStorageFolder;
                if (storageFolder != null)
                {
                    folder = new SFWinRTFolder(storageFolder);
                }
            }
            return folder;
        }

        /// <summary>
        /// Gets a list of subfolders in this folder
        /// </summary>
        /// <returns>A list of subfolders in the folder</returns>
        public async Task<IList<IFolder>> GetFoldersAsync(CancellationToken cancellationToken)
        {
            var wrtFolders = await _wrappedFolder.GetFoldersAsync().AsTask(cancellationToken).ConfigureAwait(false);
            var folders = wrtFolders.Select(f => new SFWinRTFolder(f)).ToList<IFolder>();
            return new ReadOnlyCollection<IFolder>(folders);
        }

        /// <summary>
        /// Checks whether a folder or file exists at the given location.
        /// </summary>
        /// <param name="name">The name of the file or folder to check for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task whose result is the result of the existence check.
        /// </returns>
        public async Task<ExistenceCheckResult> CheckExistsAsync(string name, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(name, "name");

            IStorageItem item = null;
            ExistenceCheckResult result = ExistenceCheckResult.NotFound;
            item = await TryGetItemAsync(name).ConfigureAwait(false);
            if (item != null)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    result = ExistenceCheckResult.FileExists;
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    result = ExistenceCheckResult.FolderExists;
                }
            }

            return result;
        }

        private async Task<IStorageItem> TryGetItemAsync(string name)
        {
            IStorageItem item = null;
#if WINDOWS_PHONE_APP
            try
            {
                item = await _wrappedFolder.GetItemAsync(name).AsTask().ConfigureAwait(false);
            }
            catch (FileNotFoundException)
            {
            }
#else // UWP, Win8RT
            var storageFolder = _wrappedFolder as StorageFolder;
            item = await storageFolder.TryGetItemAsync(name).AsTask().ConfigureAwait(false);
#endif
            return item;
        }

        /// <summary>
        /// Deletes this folder and all of its contents
        /// </summary>
        /// <returns>A task which will complete after the folder is deleted</returns>
        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            if (_isRootFolder)
            {
                throw new IOException("Cannot delete root storage folder.");
            }

            await _wrappedFolder.DeleteAsync().AsTask(cancellationToken).ConfigureAwait(false);
        }

        Windows.Storage.CreationCollisionOption GetWinRTCreationCollisionOption(PCLStorage.CreationCollisionOption option)
        {
            if (option == PCLStorage.CreationCollisionOption.GenerateUniqueName)
            {
                return Windows.Storage.CreationCollisionOption.GenerateUniqueName;
            }
            else if (option == PCLStorage.CreationCollisionOption.ReplaceExisting)
            {
                return Windows.Storage.CreationCollisionOption.ReplaceExisting;
            }
            else if (option == PCLStorage.CreationCollisionOption.FailIfExists)
            {
                return Windows.Storage.CreationCollisionOption.FailIfExists;
            }
            else if (option == PCLStorage.CreationCollisionOption.OpenIfExists)
            {
                return Windows.Storage.CreationCollisionOption.OpenIfExists;
            }
            else
            {
                throw new ArgumentException("Unrecognized CreationCollisionOption value: " + option);
            }
        }

        /// <summary>
        /// Convert IFolder to StorageFolder
        /// </summary>
        /// <param name="folder">Base folder</param>
        public static explicit operator StorageFolder(SFWinRTFolder folder)
        {
            return folder._wrappedFolder as StorageFolder;
        }

        /// <summary>
        /// Convert StorageFolder to IFolder
        /// </summary>
        /// <param name="folder">Base folder</param>
        public static explicit operator SFWinRTFolder(StorageFolder folder)
        {
            return new SFWinRTFolder(folder);
        }
    }
}
