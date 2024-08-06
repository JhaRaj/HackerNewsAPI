namespace HackerNews.Common.Cache
{
    /// <summary>
    /// ICache
    /// </summary>
    public interface ICache
    { 
        /// <summary>
        /// Get Cached item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Set Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresInSeconds"></param>
        void Set<T>(string key, T value, double expiresInSeconds = 300);

        /// <summary>
        /// Sets the with expiration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresInSeconds"></param>
        void SetWithExpiration<T>(string key, T value, double expiresInSeconds = 300);

        /// <summary>
        /// RemoveCacheItem
        /// </summary>
        /// <param name="key"></param>
        void RemoveCacheItem(string key);
    }
}
