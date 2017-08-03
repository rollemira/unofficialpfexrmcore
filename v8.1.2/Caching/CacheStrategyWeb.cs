using System;
using System.Web;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    /// This is a cache strategy that can be used in ASP.NET applications
    /// </summary>
    public sealed class CacheStrategyWeb : ICacheStrategy
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
            if (Exists(key)) Remove(key);
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
        ///     Clears the cache of all items.
        /// </summary>
        public void Clear()
        {
            var enumerator = HttpContext.Current.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                HttpContext.Current.Cache.Remove((string) enumerator.Key);
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
            try
            {
                HttpContext.Current.Cache.Remove(key);
            }
            catch
            {
                //don't do anything
            }
        }

        public void Dispose()
        {
        }
    }
}
