namespace HackerNews.Common.Http
{
    public interface IRestClient
    {
        /// <summary>
        /// Gets the specified URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<APIResult<T>> Get<T>(string uri) where T : class;
    }
}
