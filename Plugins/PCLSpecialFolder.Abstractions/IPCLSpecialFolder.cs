using System;

namespace PCLStorage.Abstractions
{
    /// <summary>
    /// Interface for PCLSpecialFolder
    /// </summary>
    public interface ISpecialFolder
    {
        /// <summary>
        /// Application folder
        /// </summary>
        IFolder App { get; }
        /// <summary>
        /// Local data folder
        /// </summary>
        IFolder Local { get; }
        /// <summary>
        /// Roaming data folder
        /// </summary>
        IFolder Roaming { get; }
        /// <summary>
        /// Tamporary data folder
        /// </summary>
        IFolder Temporary { get; }
        /// <summary>
        /// Cache folder
        /// </summary>
        IFolder Cache { get; }
        /// <summary>
        /// Documents folder
        /// </summary>
        IFolder Documents { get; }
        /// <summary>
        /// Pictures folder
        /// </summary>
        IFolder Pictures { get; }
        /// <summary>
        /// Music folder
        /// </summary>
        IFolder Music { get; }
        /// <summary>
        /// Videos folder
        /// </summary>
        IFolder Videos { get; }
    }
}
