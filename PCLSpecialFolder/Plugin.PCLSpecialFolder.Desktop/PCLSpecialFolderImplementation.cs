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
        public IFolder App
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Local data folder
        /// </summary>
        public IFolder Local
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Roaming data folder
        /// </summary>
        public IFolder Roaming
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Tamporary data folder
        /// </summary>
        public IFolder Temporary
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Cache folder
        /// </summary>
        public IFolder Cache
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Documents folder
        /// </summary>
        public IFolder Documents
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Pictures folder
        /// </summary>
        public IFolder Pictures
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Music folder
        /// </summary>
        public IFolder Music
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Videos folder
        /// </summary>
        public IFolder Videos
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
