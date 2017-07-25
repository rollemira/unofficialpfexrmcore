using System;

namespace Microsoft.Pfe.Xrm.Caching
{
    /// <summary>
    /// Cache strategy using the Strategy pattern
    /// </summary>
    public interface ICacheStrategy : IDisposable
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
        void Add<T>(T o, string key, TimeSpan? slidingExpiration = default(TimeSpan?), DateTime? absoluteExpirationUtc = default(DateTime?));

        /// <summary>
        ///     Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        ///     Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        T Get<T>(string key);

        /// <summary>
        ///     Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        void Remove(string key);
    }
}