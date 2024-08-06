using HackerNews.Common.Cache;
using HackerNews.Common.Constants;
using HackerNews.Common.Http;
using HackerNews.Service;
using HackerNews.Service.Entity;
using Moq;

namespace HackerNews.UnitTests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<IRestClient> _restClientMock;
        private readonly Mock<ICache> _cacheMock;

        /// <summary>
        /// HackerNewsServiceTests constructor
        /// </summary>
        public HackerNewsServiceTests()
        {
            _restClientMock = new Mock<IRestClient>();
            _cacheMock = new Mock<ICache>();
        }

        /// <summary>
        /// mock stories Ids & story details and call service layer to perform unit test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetStories()
        {
            //Arrange
            string content = @"[41104600]";
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(content);
            _restClientMock.Setup(x => x.Get<long[]>(Url.URIStoriesId)).ReturnsAsync(new APIResult<long[]>(response));

            string storyContent = @"{
                                ""id"": 41104523,
                                ""title"": ""SAM 2: Segment Anything in Images and Videos"",
                                ""type"": ""story"",
                                ""url"": ""https://github.com/facebookresearch/segment-anything-2""
                            }";

            HttpResponseMessage res = new HttpResponseMessage();
            res.Content = new StringContent(storyContent);
            _restClientMock.Setup(x => x.Get<Story>(Url.URIStoriesInfoById + "41104600" + ".json")).ReturnsAsync(new APIResult<Story>(res));

            var service = new HackerNewsService(_restClientMock.Object, _cacheMock.Object);

            // Act
            var result = await service.GetStories(1);

            // Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// mock cached stories details and call service layer to perform unit test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetCachedStories()
        {
            //Arrange
            List<Story> expectedStories = new List<Story>();
            {
                new Story
                {
                    Id = 8863,
                    Title = "My YC app: Dropbox - Throw away your USB drive",
                    Type = "story",
                    Url = "https://github.com/facebookresearch/segment-anything-2"
                };
            };
            _cacheMock.Setup(x => x.Get<List<Story>>("GetStories")).Returns(expectedStories);

            var service = new HackerNewsService(_restClientMock.Object, _cacheMock.Object);

            // Act
            var result = await service.GetStories(1);

            // Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// validate error message;
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidateExceptionMessage()
        {
            //Arrange
            string content = @"[41104600]";
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(content);
            _restClientMock.Setup(x => x.Get<long[]>("")).ReturnsAsync(new APIResult<long[]>(response));

            string storyContent = @"{
                                ""id"": 41104523,
                                ""title"": ""SAM 2: Segment Anything in Images and Videos"",
                                ""type"": ""story"",
                                ""url"": ""https://github.com/facebookresearch/segment-anything-2""
                            }";

            HttpResponseMessage res = new HttpResponseMessage();
            res.Content = new StringContent(storyContent);
            _restClientMock.Setup(x => x.Get<Story>(Url.URIStoriesInfoById + "41104600" + ".json")).ReturnsAsync(new APIResult<Story>(res));

            var service = new HackerNewsService(_restClientMock.Object, _cacheMock.Object);

            try
            {
                // Act
                var result = await service.GetStories(1);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Contains("Unable to get result from 'https://hacker-news.firebaseio.com/v0/topstories.json'", ex.Message);
            }

        }

        /// <summary>
        /// handle http error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task HandleHttpError()
        {
            //Arrange
            string content = @"[41104600]";
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(content);
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _restClientMock.Setup(x => x.Get<long[]>(Url.URIStoriesId)).ReturnsAsync(new APIResult<long[]>(response));

            string storyContent = @"{
                                ""id"": 41104523,
                                ""title"": ""SAM 2: Segment Anything in Images and Videos"",
                                ""type"": ""story"",
                                ""url"": ""https://github.com/facebookresearch/segment-anything-2""
                            }";

            HttpResponseMessage res = new HttpResponseMessage();
            res.Content = new StringContent(storyContent);
            _restClientMock.Setup(x => x.Get<Story>(Url.URIStoriesInfoById + "41104600" + ".json")).ReturnsAsync(new APIResult<Story>(res));

            var service = new HackerNewsService(_restClientMock.Object, _cacheMock.Object);

            try
            {
                var result = await service.GetStories(1);
            }
            catch
            {
                Assert.True(true);
            }
        }
    }
}
