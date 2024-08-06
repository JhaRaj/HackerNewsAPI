using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace HackerNews.Common.Cache
{
    [ExcludeFromCodeCoverage]
    public class Cache : ICache
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Cache
        /// </summary>
        /// <param name="cache"></param>
        public Cache(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Get Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// Set Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresInSeconds"></param>
        public void Set<T>(string key, T value, double expiresInSeconds = 300)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(expiresInSeconds)
            };

            _cache.Set(key, value, options);
        }

        /// <summary>
        /// Sets the with expiration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresInSeconds"></param>
        public void SetWithExpiration<T>(string key, T value, double expiresInSeconds = 300)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expiresInSeconds)
            };

            _cache.Set(key, value, options);
        }

        /// <summary>
        /// RemoveCacheItem
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCacheItem(string key)
        {
            _cache.Remove(key);
        }
    }
}
