using System.Diagnostics.CodeAnalysis;

namespace HackerNews.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class APIExceptions : Exception
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public Error error { get; private set; } = new Error();

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public APIExceptions(Error err) : base(err?.ErrorDescription)
        {
            error = err;
        }
    }
}
