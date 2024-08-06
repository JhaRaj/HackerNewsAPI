using HackerNews.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HackerNews.WebApi.Middleware
{
    /// <summary>
    /// ErrorHandling Middleware
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string JSON_CONTENT_TYPE = "application/json; charset=utf-8";

        /// <summary>
        ///  Middleware constructor
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///   Middleware logic
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task<int> HandleExceptionAsync(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case APIExceptions ex:
                    {
                        var result = new ErrorResponse
                        {
                            ErrorCode = (int) ex.error.ErrorCode,
                            ErrorDescription = exception.Message
                        };
                        context.Response.ContentType = JSON_CONTENT_TYPE;
                        context.Response.StatusCode = (int)ex.error.ErrorCode;
                        context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                        return Task.FromResult(0);
                    }
            }

            var errorCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = JSON_CONTENT_TYPE;
            context.Response.StatusCode = (int)errorCode;
            context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
            {
                ErrorCode = (int)errorCode,
                ErrorDescription = exception.Message
            }));
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Middleware Extensions
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

    /// <summary>
    /// ErrorResponse
    /// </summary>
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
