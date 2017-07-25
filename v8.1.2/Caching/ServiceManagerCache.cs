using System;
using System.Web;

namespace Microsoft.Pfe.Xrm.Caching
{
    /// <summary>
    ///     A wrapper for the chosen cache strategy
    /// </summary>
    internal sealed class ServiceManagerCache
    {
        private static ICacheStrategy _cacheStrategy;

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
            _cacheStrategy.Add(o, key, slidingExpiration, absoluteExpiration);
        }

        /// <summary>
        ///     Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Remove(string key)
        {
            _cacheStrategy.Remove(key);
            HttpContext.Current.Cache.Remove(key);
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
    }
}
