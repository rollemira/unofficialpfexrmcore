namespace Microsoft.Pfe.Xrm.Caching
{
    /// <summary>
    /// The different cache strategies to choose from
    /// </summary>
    public static class CacheStrategies
    {
        /// <summary>
        /// A cache strategy for non-web applications
        /// </summary>
        public static ICacheStrategy MemoryCacheStrategy => new MemoryCacheStrategy();
        /// <summary>
        /// Turns off caching
        /// </summary>
        public static ICacheStrategy None => new NoCacheStrategy();
        /// <summary>
        /// A cache strategy for web applications
        /// </summary>
        public static ICacheStrategy WebCacheStrategy => new WebCacheStrategy();
    }
}
