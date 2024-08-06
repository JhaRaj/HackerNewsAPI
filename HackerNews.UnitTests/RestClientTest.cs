using HackerNews.Common.Constants;
using HackerNews.Common.Http;


namespace HackerNews.UnitTests
{
    public class RestClientTest
    {
        RestClient _client = null;

        /// <summary>
        ///  initialise the RestClient when HtppClient is Null
        /// </summary>
        [Fact]
        public void InitialiseRestClientWhentHttpClientIsNull()
        {
            _client = new RestClient(new Uri(Url.URIStoriesId));
            Assert.NotNull(_client);    
        }

        /// <summary>
        ///  initialise the RestClient when HtppClient is Not Null
        /// </summary>
        [Fact]
        public void InitialiseRestClientWhentHttpClientIsNotNull()
        {
            _client = new RestClient(new Uri(Url.URIStoriesId),TimeSpan.FromSeconds(300),new HttpClient());
            Assert.NotNull(_client);
        }

    }
}
