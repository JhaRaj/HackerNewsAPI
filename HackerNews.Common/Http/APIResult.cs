using Newtonsoft.Json;
using System.Net;

namespace HackerNews.Common.Http{
    /// <summary>
    /// Encapsulates the result returned by the Hacker API
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// The response
        /// </summary>
        private readonly HttpResponseMessage _response;
        /// <summary>
        /// The raw data
        /// </summary>
        private string _rawData;

        /// <summary>
        /// HTTP Status Code.
        /// </summary>
        public HttpStatusCode StatusCode => _response.StatusCode;

        /// <summary>
        ///  Gets the response.
        /// </summary>
        public HttpResponseMessage Response
        {
            get
            {
                return _response;
            }
        }

        /// <summary>
        ///  Initializes a new instance of the APIResult class.
        /// </summary>
        /// <param name="responseMessage"></param>
        public APIResult(HttpResponseMessage responseMessage) => _response = responseMessage;

        /// <summary>
        ///  Gets the raw data.
        /// </summary>
        public string RawData => _rawData ?? GetRawData();

        /// <summary>
        ///  Gets the raw data.
        /// </summary>
        /// <returns>raw data.</returns>
        private string GetRawData()
        {
            var stream = _response.Content.ReadAsStreamAsync().Result;

            if (stream == null)
                return _rawData;

            using (var reader = new StreamReader(stream))
            {
                _rawData = reader.ReadToEnd();
            }

            return _rawData;
        }

        /// <summary>
        /// Gets a value indicating whether [success].
        /// </summary>
        public bool Success => (HttpStatusCode.Created == StatusCode || HttpStatusCode.OK == StatusCode);
    }
    public class APIResult<T> : APIResult
    {
        /// <summary>
        ///  Initializes a new instance of the APIResult class.
        /// </summary>
        /// <param name="response">response</param>
        public APIResult(HttpResponseMessage response) : base(response)
        {
        }
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public T Entity => JsonConvert.DeserializeObject<T>(RawData);
    }
}
