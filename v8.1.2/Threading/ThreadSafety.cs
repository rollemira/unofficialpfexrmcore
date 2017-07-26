using System;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    /// A class to help with threading stuff
    /// </summary>
    internal static class ThreadSafety
    {
        /// <summary>
        /// An object to lock on in the assembly
        /// </summary>
        public static object SyncRoot = new Object();
    }
}
