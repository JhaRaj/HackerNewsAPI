using System.Net;

namespace HackerNews.Common.Exceptions
{
    /// <summary>
    /// Error
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public HttpStatusCode ErrorCode { get; set; }
        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// The error description.
        /// </value>
        public string ErrorDescription { get; set; }
    }
}
