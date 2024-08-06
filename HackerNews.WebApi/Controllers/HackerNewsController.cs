using HackerNews.Service;
using HackerNews.Service.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _service;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// HackerNewsController
        /// </summary>
        /// <param name="service"></param>
        public HackerNewsController(IHackerNewsService service,IConfiguration configuration)
        {
            _service = service;
            _configuration= configuration;
        }

        /// <summary>
        /// Get Stories
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStories")]
        public async Task<List<Story>> GetStories()
        {
            int storiesCount = Convert.ToInt32(_configuration.GetSection("RecordsCount").Value);
            return await _service.GetStories(storiesCount);
        }
    }
}
