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
        public static ICacheStrategy Memory => new CacheStrategyMemory();
        /// <summary>
        /// Turns off caching
        /// </summary>
        public static ICacheStrategy None => new CacheStrategyNone();
        /// <summary>
        /// A cache strategy for web applications
        /// </summary>
        public static ICacheStrategy Web => new CacheStrategyWeb();
    }
}
