using PCLStorage.Abstractions;
using System;
using Windows.Storage;

namespace PCLStorage
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class SpecialFolderImplementation : ISpecialFolder
    {
        /// <summary>
        /// Application folder
        /// </summary>
        public IFolder App { get { return _app.Value; } }
        private Lazy<IFolder> _app = new Lazy<IFolder>(() => GetFolder(Windows.ApplicationModel.Package.Current.InstalledLocation));

        /// <summary>
        /// Local data folder
        /// </summary>
        public IFolder Local { get { return _local.Value; } }
        private Lazy<IFolder> _local = new Lazy<IFolder>(() => GetFolder(ApplicationData.Current.LocalFolder));

        /// <summary>
        /// Roaming data folder
        /// </summary>
        public IFolder Roaming { get { return _roaming.Value; } }
        private Lazy<IFolder> _roaming = new Lazy<IFolder>(() => GetFolder(ApplicationData.Current.RoamingFolder));

        /// <summary>
        /// Tamporary data folder
        /// </summary>
        public IFolder Temporary { get { return _temporary.Value; } }
        private Lazy<IFolder> _temporary = new Lazy<IFolder>(() => GetFolder(ApplicationData.Current.TemporaryFolder));

        /// <summary>
        /// Cache folder
        /// </summary>
        public IFolder Cache { get { return _cache.Value; } }
#if WINDOWS_PHONE_APP || WINDOWS_UWP
        private Lazy<IFolder> _cache = new Lazy<IFolder>(() => GetFolder(ApplicationData.Current.LocalCacheFolder));
#else // Win81RT
        private Lazy<IFolder> _cache = new Lazy<IFolder>(() => null);
#endif

        /// <summary>
        /// Documents folder
        /// </summary>
        public IFolder Documents { get { return _documents.Value; } }
        private Lazy<IFolder> _documents = new Lazy<IFolder>(() => GetFolder(KnownFolders.DocumentsLibrary));

        /// <summary>
        /// Pictures folder
        /// </summary>
        public IFolder Pictures { get { return _pictures.Value; } }
        private Lazy<IFolder> _pictures = new Lazy<IFolder>(() => GetFolder(KnownFolders.PicturesLibrary));

        /// <summary>
        /// Music folder
        /// </summary>
        public IFolder Music { get { return _music.Value; } }
        private Lazy<IFolder> _music = new Lazy<IFolder>(() => GetFolder(KnownFolders.MusicLibrary));

        /// <summary>
        /// Videos folder
        /// </summary>
        public IFolder Videos { get { return _videos.Value; } }
        private Lazy<IFolder> _videos = new Lazy<IFolder>(() => GetFolder(KnownFolders.VideosLibrary));

        private static IFolder GetFolder(StorageFolder storageFolder)
        {
            IFolder folder = null;
            folder = new SFWinRTFolder(storageFolder, isRootFolder: true);
            return folder;
        }
    }
}
