using Newtonsoft.Json;

namespace HackerNews.Service.Entity
{
    /// <summary>
    /// Story
    /// </summary>
    public class Story
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
