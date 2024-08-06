using HackerNews.Service.Entity;

namespace HackerNews.Service
{
    /// <summary>
    /// IHackerNewsService
    /// </summary>
    public interface IHackerNewsService
    {
        Task<List<Story>> GetStories(int itemsCount);
    }
}
