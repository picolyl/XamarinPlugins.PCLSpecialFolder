using PCLStorage;
using Plugin.PCLSpecialFolder.Abstractions;
using System;

namespace Plugin.PCLSpecialFolder
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class PCLSpecialFolderImplementation : IPCLSpecialFolder
    {
        /// <summary>
        /// Application folder
        /// </summary>
        public IFolder App { get { return _app.Value; } }
        private Lazy<IFolder> _app = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.App));

        /// <summary>
        /// Local data folder
        /// </summary>
        public IFolder Local { get { return _local.Value; } }
        private Lazy<IFolder> _local = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Local));

        /// <summary>
        /// Roaming data folder
        /// </summary>
        public IFolder Roaming { get { return _roaming.Value; } }
        private Lazy<IFolder> _roaming = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Roaming));

        /// <summary>
        /// Tamporary data folder
        /// </summary>
        public IFolder Temporary { get { return _temporary.Value; } }
        private Lazy<IFolder> _temporary = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Temporary));

        /// <summary>
        /// Cache folder
        /// </summary>
        public IFolder Cache { get { return _cache.Value; } }
        private Lazy<IFolder> _cache = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Cache));

        /// <summary>
        /// Documents folder
        /// </summary>
        public IFolder Documents { get { return _documents.Value; } }
        private Lazy<IFolder> _documents = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Documents));

        /// <summary>
        /// Pictures folder
        /// </summary>
        public IFolder Pictures { get { return _pictures.Value; } }
        private Lazy<IFolder> _pictures = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Pictures));

        /// <summary>
        /// Music folder
        /// </summary>
        public IFolder Music { get { return _music.Value; } }
        private Lazy<IFolder> _music = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Music));

        /// <summary>
        /// Videos folder
        /// </summary>
        public IFolder Videos { get { return _videos.Value; } }
        private Lazy<IFolder> _videos = new Lazy<IFolder>(() => GetFolder(FolderPath.FolderPath.Current.Videos));

        private static IFolder GetFolder(string path)
        {
            IFolder folder = null;
            if (!string.IsNullOrWhiteSpace(path))
            {
                folder = new FileSystemFolder(path, canDelete: false);
            }
            return folder;
        }
    }
}
