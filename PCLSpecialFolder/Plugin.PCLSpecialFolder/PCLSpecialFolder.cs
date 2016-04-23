using Plugin.PCLSpecialFolder.Abstractions;
using System;

namespace Plugin.PCLSpecialFolder
{
    /// <summary>
    /// Cross platform PCLSpecialFolder implemenations
    /// </summary>
    public class PCLSpecialFolder
    {
        static Lazy<IPCLSpecialFolder> Implementation = new Lazy<IPCLSpecialFolder>(() => CreatePCLSpecialFolder(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IPCLSpecialFolder Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IPCLSpecialFolder CreatePCLSpecialFolder()
        {
#if PORTABLE
            return null;
#else
            return new PCLSpecialFolderImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
