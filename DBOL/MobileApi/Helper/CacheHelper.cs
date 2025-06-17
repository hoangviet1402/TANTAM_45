using System;
using System.Web;
using System.Web.Caching;

namespace TanTamApi.Helper
{
    public static class CacheHelper
    {
        /// <summary>
        ///     Insert value into the cache using
        ///     appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T o, string key)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                //DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///     Insert value into the cache using
        ///     appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="minutes">Minutes expire</param>
        public static void Add<T>(T o, string key, int minutes)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///     Insert value into the cache using
        ///     appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="seconds">Seconds expire</param>
        public static void AddSecond<T>(T o, string key, int seconds)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddSeconds(seconds),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///     Insert value into the cache using
        ///     appropriate name/value pairs with callback
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="minutes">Minutes expire</param>
        /// <param name="callback">CacheItemRemovedCallback</param>
        public static void Add<T>(T o, string key, int minutes, CacheItemRemovedCallback callback)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                callback);
        }

        /// <summary>
        ///     Insert value into the cache using
        ///     appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="expireTime">Time expire</param>
        public static void Add<T>(T o, string key, DateTime expireTime)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                expireTime,
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///     Insert value into the cache nerver exprite
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="key"></param>
        public static void AddForever<T>(T o, string key)
        {
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///     Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        ///     Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        ///     Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)HttpRuntime.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}