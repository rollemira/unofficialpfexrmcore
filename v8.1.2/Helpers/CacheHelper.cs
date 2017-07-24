using System;
using System.Web;
using System.Web.Caching;

/// <summary>
///     Uses the HttpContext.Current.Cache to cache items
///     (May want to use MemoryCache??)
/// </summary>
internal sealed class CacheHelper
{
    /// <summary>
    ///     Insert value into the cache using
    ///     appropriate name/value pairs
    /// </summary>
    /// <typeparam name="T">Type of cached item</typeparam>
    /// <param name="o">Item to be cached</param>
    /// <param name="key">Name of item</param>
    /// <param name="slidingExpiration">Sliding cache expiration</param>
    /// <param name="absoluteExpiration">Absolute cache expiration</param>
    public static void Add<T>(T o, string key, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
    {
        if (Exists(key)) return;
        if (slidingExpiration.HasValue)
            HttpContext.Current.Cache.Insert(key, o, null, Cache.NoAbsoluteExpiration, slidingExpiration.Value);
        else if (absoluteExpiration.HasValue)
            HttpContext.Current.Cache.Insert(key, o, null, absoluteExpiration.Value, Cache.NoSlidingExpiration);
    }

    /// <summary>
    ///     Remove item from cache
    /// </summary>
    /// <param name="key">Name of cached item</param>
    public static void Remove(string key)
    {
        HttpContext.Current.Cache.Remove(key);
    }

    /// <summary>
    ///     Check for item in cache
    /// </summary>
    /// <param name="key">Name of cached item</param>
    /// <returns></returns>
    public static bool Exists(string key)
    {
        return HttpContext.Current.Cache[key] != null;
    }

    /// <summary>
    ///     Retrieve cached item
    /// </summary>
    /// <typeparam name="T">Type of cached item</typeparam>
    /// <param name="key">Name of cached item</param>
    /// <returns>Cached item as type</returns>
    public static T Get<T>(string key)
    {
        try
        {
            return (T) HttpContext.Current.Cache[key];
        }
        catch
        {
            return default(T);
        }
    }
}