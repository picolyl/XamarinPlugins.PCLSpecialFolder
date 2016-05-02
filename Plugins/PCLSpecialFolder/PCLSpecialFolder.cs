using PCLStorage.Abstractions;
using System;

namespace PCLStorage
{
    /// <summary>
    /// Cross platform PCLSpecialFolder implemenations
    /// </summary>
    public class SpecialFolder
    {
        static Lazy<ISpecialFolder> Implementation = new Lazy<ISpecialFolder>(() => CreatePCLSpecialFolder(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ISpecialFolder Current
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

        static ISpecialFolder CreatePCLSpecialFolder()
        {
#if PORTABLE
            return null;
#else
            return new SpecialFolderImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
