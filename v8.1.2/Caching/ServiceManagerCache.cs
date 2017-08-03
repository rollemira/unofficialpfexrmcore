using System;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    ///     A wrapper for the chosen cache strategy
    /// </summary>
    public sealed class ServiceManagerCache : IDisposable
    {
        private readonly ICacheStrategy _cacheStrategy;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheStrategy">The chosen cache strategy</param>
        /// <see cref="CacheStrategies"/>
        public ServiceManagerCache(ICacheStrategy cacheStrategy)
        {
            _cacheStrategy = cacheStrategy;
        }

        /// <summary>
        ///     Add value into the cache
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="slidingExpiration">Sliding cache expiration</param>
        /// <param name="absoluteExpiration">Absolute cache expiration</param>
        /// <remarks>
        /// Absolute expirations should ALWAYS be UTC, and should override sliding expirations
        /// </remarks>
        public void Add<T>(T o, string key, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            //only add it if we're given an expiration
            if (slidingExpiration.HasValue || absoluteExpiration.HasValue)
                _cacheStrategy.Add(o, key, slidingExpiration, absoluteExpiration);
        }

        /// <summary>
        ///     Clears the cache of all items.
        /// </summary>
        public void Clear()
        {
            _cacheStrategy.Clear();
        }

        /// <summary>
        ///     Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Remove(string key)
        {
            _cacheStrategy.Remove(key);
        }

        /// <summary>
        ///     Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return _cacheStrategy.Exists(key);
        }

        /// <summary>
        ///     Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T Get<T>(string key)
        {
            return _cacheStrategy.Get<T>(key);
        }

        public void Dispose()
        {
            _cacheStrategy?.Dispose();
        }
    }
}
