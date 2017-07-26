using System;
using System.Runtime.Caching;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    /// This is a cache strategy that can be used for non-web
    /// </summary>
    public sealed class CacheStrategyMemory : ICacheStrategy
    {
        private static readonly MemoryCache MemoryCache = new MemoryCache("pfexrmcore.cache");

        /// <summary>
        ///     Add value into the cache
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="slidingExpiration">Sliding cache expiration</param>
        /// <param name="absoluteExpirationUtc">Absolute cache expiration (Should be in UTC)</param>
        /// <remarks>
        /// Absolute expirations should ALWAYS be UTC, and should override sliding expirations
        /// </remarks>
        public void Add<T>(T o, string key, TimeSpan? slidingExpiration = null, DateTime? absoluteExpirationUtc = null)
        {
            if (Exists(key)) Remove(key);
            var cachePolicy = new CacheItemPolicy();
            //absolute expiration should have priority
            if (absoluteExpirationUtc.HasValue)
            {
                DateTimeOffset offset = absoluteExpirationUtc.Value;
                cachePolicy.AbsoluteExpiration = offset;
            }
            else if (slidingExpiration.HasValue)
            {
                cachePolicy.SlidingExpiration = slidingExpiration.Value;

            }
            MemoryCache.Add(new CacheItem(key, o), cachePolicy);
        }

        /// <summary>
        ///     Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            try
            {
                object item = MemoryCache[key];
                return item == null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T Get<T>(string key)
        {
            try
            {
                return (T)MemoryCache[key];
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        ///     Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Remove(string key)
        {
            try
            {
                MemoryCache.Remove(key);
            }
            catch (Exception)
            {
                //don't do anything
            }
        }

        private void ReleaseUnmanagedResources()
        {
            //Release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                MemoryCache?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CacheStrategyMemory()
        {
            Dispose(false);
        }
    }
}