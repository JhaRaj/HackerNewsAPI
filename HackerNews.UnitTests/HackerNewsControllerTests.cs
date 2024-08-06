using HackerNews.Service;
using HackerNews.Service.Entity;
using HackerNews.WebApi.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HackerNews.UnitTests
{
    /// <summary>
    /// HackerNewsControllerTests
    /// </summary>
    public class HackerNewsControllerTests
    {
        private readonly Mock<IHackerNewsService> _hackerNewsServiceMock;

        /// <summary>
        /// HackerNewsControllerTests constructor
        /// </summary>
        public HackerNewsControllerTests()
        {
            _hackerNewsServiceMock = new Mock<IHackerNewsService>();
        }

        /// <summary>
        /// Unit Test for GetStories Details
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetStoriesDetails()
        {
            // Arrange
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

            // Act
            var result = MockGetStoriesDetailsCall(expectedStories);

            // Assert
            Assert.NotNull(result.Result);
        }

        /// <summary>
        /// To Test empty result in GetStoriesDetails
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetStoriesDetails_EmptyResult()
        {
            // Arrange
            List<Story> expectedStories = new List<Story>();

            // Act
            var result = MockGetStoriesDetailsCall(expectedStories);

            // Assert
            if (result.Result.Count == 0)
            {
                Assert.Empty(expectedStories);
            }
        }

        /// <summary>
        /// Mock GetStoriesDetails 
        /// </summary>
        /// <param name="lstStories"></param>
        /// <returns></returns>
        private async Task<List<Story>> MockGetStoriesDetailsCall(List<Story> lstStories)
        {
            _hackerNewsServiceMock.Setup(x => x.GetStories(1)).ReturnsAsync(lstStories);

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("1");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("RecordsCount")).Returns(mockSection.Object);

            var controller = new HackerNewsController(_hackerNewsServiceMock.Object, mockConfig.Object);

            return await controller.GetStories();
        }
    }

}