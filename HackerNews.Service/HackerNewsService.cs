using HackerNews.Common.Cache;
using HackerNews.Common.Constants;
using HackerNews.Common.Exceptions;
using HackerNews.Common.Http;
using HackerNews.Service.Entity;
using System.Net;

namespace HackerNews.Service
{
    /// <summary>
    /// HackerNewsService
    /// </summary>
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IRestClient _restClient;
        private readonly ICache _Cache;

        private readonly string CacheKey = "GetStories";

        /// <summary>
        /// HackerNewsService constructor
        /// </summary>
        /// <param name="restClient"></param>
        public HackerNewsService(IRestClient restClient, ICache cache)
        {
            _restClient = restClient;
            _Cache = cache;
        }

        /// <summary>
        /// To fetch Stories
        /// </summary>
        /// <returns></returns>
        public async Task<List<Story>> GetStories(int storiesCount)
        {
            //get data from cache
            var keysFromCache = _Cache.Get<List<Story>>(CacheKey);
            if (keysFromCache != null)
            {
                return await Task.FromResult(keysFromCache);
            }

            string storiesIds_Url = Url.URIStoriesId;
            //api call to get stories ids
            var response = await _restClient.Get<long[]>(storiesIds_Url);
            ThrowOnError(response, storiesIds_Url);

            //getting stories ids from api response
            long[] storiesId = response.Entity;

            //api call to get stories details by id
            var tasks = storiesId.Select(id => GetStoriesData(id));
            var stories = await Task.WhenAll(tasks);

            //list to store stories details
            List<Story> lstStories = new List<Story>();
            lstStories.AddRange(stories.Where(story => !string.IsNullOrEmpty(story.Url)).Take(200).ToList());

            //set data to cache
            _Cache.Set(CacheKey, lstStories, CacheDuration.FiveMinutes);

            return lstStories;
        }

        private void ThrowOnError<T>(APIResult<T> result, string url)
        {
            if (result == null)
            {
                throw new Exception($"Unable to get result from '{url}'");
            }

            if (result.StatusCode == HttpStatusCode.OK)
                return;

            Error err = new Error() { ErrorCode = result.StatusCode, ErrorDescription = result.RawData };
            throw new APIExceptions(err);
        }
        private async Task<Story> GetStoriesData(long id)
        {
            string storiesDetails_Url = Url.URIStoriesInfoById + id + ".json";
            var result = await _restClient.Get<Story>(storiesDetails_Url);
            ThrowOnError(result, storiesDetails_Url);
            return result.Entity;
        }
    }
}
