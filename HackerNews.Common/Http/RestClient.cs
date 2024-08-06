namespace HackerNews.Common.Http
{
    /// <summary>
    /// Http RestClient
    /// </summary>
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// RestClient constructor
        /// </summary>
        /// <param name="address"></param>
        /// <param name="timeOut"></param>
        /// <param name="httpClient"></param>
        public RestClient(Uri address = null, TimeSpan? timeOut = null, HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        if (errors == System.Net.Security.SslPolicyErrors.None)
                        {
                            return true; // The certificate is valid
                        }
                        return false;
                    }
                };

                _httpClient = new HttpClient(httpClientHandler, true)
                {
                    BaseAddress = address
                };
            }
            else
            {
                _httpClient = httpClient;
            }

            if (timeOut.HasValue)
                _httpClient.Timeout = timeOut.Value;
        }

        /// <summary>
        /// Get Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<APIResult<T>> Get<T>(string uri) where T : class
        {
            var response = await _httpClient.GetAsync(uri);
            return new APIResult<T>(response);
        }
    }
}

