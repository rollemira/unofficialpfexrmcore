using System;
using System.Web;

namespace Microsoft.Pfe.Xrm.Caching
{
    /// <summary>
    /// This is a cache strategy that can be used in ASP.NET applications
    /// </summary>
    public sealed class WebCacheStrategy : ICacheStrategy
    {
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
            if (Exists(key)) return;
            //absolute expiration should have priority
            if (absoluteExpirationUtc.HasValue)
            {
                HttpContext.Current.Cache.Insert(key, o, null, absoluteExpirationUtc.Value, System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else if (slidingExpiration.HasValue)
            {
                HttpContext.Current.Cache.Insert(key, o, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration.Value);
            }
        }

        /// <summary>
        ///     Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
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
                return (T)HttpContext.Current.Cache[key];
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
            HttpContext.Current.Cache.Remove(key);
        }

        public void Dispose()
        {
        }
    }
}
