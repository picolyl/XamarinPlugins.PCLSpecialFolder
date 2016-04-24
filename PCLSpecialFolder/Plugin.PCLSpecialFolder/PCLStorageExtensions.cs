using PCLStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.PCLSpecialFolder
{
    /// <summary>
    /// Provides extension methods for the <see cref="IFile"/> and <see cref="IFolder"/> class
    /// </summary>
    public static class PCLStorageExtensions
    {
        /// <summary>
        /// Creates a subfolder in this folder
        /// </summary>
        /// <param name="folder">Target folder</param>
        /// <param name="desiredName">The name of the folder to create</param>
        /// <param name="option">Specifies how to behave if the specified folder already exists</param>
        /// <returns>The newly created folder</returns>
        public static IFolder CreateFolder(this IFolder folder, string desiredName, CreationCollisionOption option)
        {
            return folder.CreateFolderAsync(desiredName, option).Result;
        }

        /// <summary>
        /// Gets a file in this folder
        /// </summary>
        /// <param name="folder">Target folder</param>
        /// <param name="name">The name of the file to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested file, or null if it does not exist</returns>
        public static async Task<IFile> TryGetFileAsync(this IFolder folder, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
#if NETFX_CORE
            var sfFolder = folder as SFWinRTFolder;
            if (sfFolder != null)
            {
                return await sfFolder.TryGetFileAsync(name, cancellationToken).ConfigureAwait(false);
            }
#endif // NETFX_CORE
            IFile newFile = null;
            if (await folder.CheckExistsAsync(name).ConfigureAwait(false) == ExistenceCheckResult.FileExists)
            {
                newFile = await folder.GetFileAsync(name, cancellationToken).ConfigureAwait(false);
            }
            return newFile;
        }

        /// <summary>
        /// Gets a folder in this folder
        /// </summary>
        /// <param name="folder">Target folder</param>
        /// <param name="name">The name of the folder to get</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested folder, or null if it does not exist</returns>
        public static async Task<IFolder> TryGetFolderAsync(this IFolder folder, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
#if NETFX_CORE
            var sfFolder = folder as SFWinRTFolder;
            if (sfFolder != null)
            {
                return await sfFolder.TryGetFolderAsync(name, cancellationToken).ConfigureAwait(false);
            }
#endif // NETFX_CORE
            IFolder newFolder = null;
            if (await folder.CheckExistsAsync(name).ConfigureAwait(false) == ExistenceCheckResult.FolderExists)
            {
                newFolder = await folder.GetFolderAsync(name, cancellationToken).ConfigureAwait(false);
            }
            return newFolder;
        }

        /// <summary>
        /// Deletes file or folder and all of its contents
        /// </summary>
        /// <param name="folder">Target folder</param>
        /// <param name="name">The name of the file or folder to delete</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task which will complete after deleted</returns>
        public static async Task DeleteAsync(this IFolder folder, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var deleteFolder = await folder.TryGetFolderAsync(name, cancellationToken).ConfigureAwait(false);
            if (deleteFolder != null)
            {
                await deleteFolder.DeleteAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var deleteFile = await folder.TryGetFileAsync(name, cancellationToken).ConfigureAwait(false);
                if (deleteFile != null)
                {
                    await deleteFile.DeleteAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Reads the contents of a file as a byte[]
        /// </summary>
        /// <param name="file">The file to read </param>
        /// <returns>The contents of the file</returns>
        public static async Task<byte[]> ReadAllBytesAsync(this IFile file)
        {
            byte[] bytes = null;
#if NETFX_CORE
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(storageFile).AsTask().ConfigureAwait(false);
            if (buffer?.Length > 0)
            {
                bytes = buffer.ToArray();
            }
            else
            {
                bytes = new byte[] { };
            }
#elif PORTABLE
            await Task.Yield();
#else // Desktop/Android/iOS
            bytes = await Task.FromResult(System.IO.File.ReadAllBytes(file.Path)).ConfigureAwait(false);
#endif
            return bytes;
        }

        /// <summary>
        /// Writes byte[] to a file, overwriting any existing data
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="bytes">The content to write to the file</param>
        /// <returns>A task which completes when the write operation finishes</returns>
        public static async Task WriteAllBytesAsync(this IFile file, byte[] bytes)
        {
#if NETFX_CORE
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
            await Windows.Storage.FileIO.WriteBytesAsync(storageFile, bytes).AsTask().ConfigureAwait(false);
#elif PORTABLE
            await Task.Yield();
#else // Desktop/Android/iOS
            await Task.Run(() => System.IO.File.WriteAllBytes(file.Path, bytes)).ConfigureAwait(false);
#endif
        }

        /// <summary>
        /// Reads the contents of a file as a lines
        /// </summary>
        /// <param name="file">The file to read </param>
        /// <returns>The contents of the file</returns>
        public static async Task<IEnumerable<string>> ReadAllLinesAsync(this IFile file)
        {
            IEnumerable<string> strings = null;
#if NETFX_CORE
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
            strings = await Windows.Storage.FileIO.ReadLinesAsync(storageFile).AsTask().ConfigureAwait(false);
#elif PORTABLE
            await Task.Yield();
#else // Desktop/Android/iOS
            strings = await Task.FromResult(System.IO.File.ReadAllLines(file.Path)).ConfigureAwait(false);
#endif
            return strings;
        }

        /// <summary>
        /// Writes lines to a file, overwriting any existing data
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <param name="lines">The content to write to the file</param>
        /// <returns>A task which completes when the write operation finishes</returns>
        public static async Task WriteAllLinesAsync(this IFile file, IEnumerable<string> lines)
        {
#if NETFX_CORE
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
            await Windows.Storage.FileIO.WriteLinesAsync(storageFile, lines).AsTask().ConfigureAwait(false);
#elif PORTABLE
            await Task.Yield();
#else // Desktop/Android/iOS
            await Task.Run(() => System.IO.File.WriteAllLines(file.Path, lines)).ConfigureAwait(false);
#endif
        }

#if NETFX_CORE
        /// <summary>
        /// Convert from IFile to StorageFile
        /// </summary>
        /// <param name="file">Target file</param>
        /// <returns>Converted file</returns>
        public static Task<Windows.Storage.StorageFile> ToStorageFileAsync(this IFile file)
        {
            return Windows.Storage.StorageFile.GetFileFromPathAsync(file.Path).AsTask();
        }

        /// <summary>
        /// Convert from StorageFile to IFile
        /// </summary>
        /// <param name="storageFile">Target file</param>
        /// <returns>Converted file</returns>
        public static Task<IFile> ToPCLFileAsync(this Windows.Storage.StorageFile storageFile)
        {
            return FileSystem.Current.GetFileFromPathAsync(storageFile.Path);
        }

        /// <summary>
        /// Convert from IFolder to StorageFolder
        /// </summary>
        /// <param name="folder">Target folder</param>
        /// <returns>Converted folder</returns>
        public static Task<Windows.Storage.StorageFolder> ToStorageFolderAsync(this IFolder folder)
        {
            var sfFolder = folder as SFWinRTFolder;
            if (sfFolder != null)
            {
                var storageFolder = (Windows.Storage.StorageFolder)sfFolder;
                return Task.FromResult(storageFolder);
            }
            else
            {
                return Windows.Storage.StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask();
            }
        }

        /// <summary>
        /// Convert from StorageFolder to IFolder
        /// </summary>
        /// <param name="storageFolder">Target folder</param>
        /// <returns>Converted folder</returns>
        public static Task<IFolder> ToPCLFolderAsync(this Windows.Storage.StorageFolder storageFolder)
        {
            if (string.IsNullOrEmpty(storageFolder.Path))
            {
                var sfFolder = new SFWinRTFolder(storageFolder);
                return Task.FromResult(sfFolder as IFolder);
            }
            else
            {
                return FileSystem.Current.GetFolderFromPathAsync(storageFolder.Path);
            }
        }
#endif // NETFX_CORE
    }
}
