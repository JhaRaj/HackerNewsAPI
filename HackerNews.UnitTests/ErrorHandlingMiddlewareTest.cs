using HackerNews.Common.Exceptions;
using HackerNews.WebApi.Middleware;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HackerNews.UnitTests
{
    public class ErrorHandlingMiddlewareTest
    {
        [Fact]
        public async Task WhenExceptionIsRaised()
        {
            // Arrange
            var middleware = new ErrorHandlingMiddleware((innerHttpContext) =>
            {
                throw new Exception();
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

            //Assert
            Assert.Equal(500, objResponse?.ErrorCode);
        }

        [Fact]
        public async Task WhenAPIExceptionsIsRaised()
        {
            // Arrange
            var middleware = new ErrorHandlingMiddleware((innerHttpContext) =>
            {
                throw new APIExceptions(new Error() { ErrorCode=System.Net.HttpStatusCode.Forbidden,ErrorDescription="forbidden"});
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

            //Assert
            Assert.Equal(403, objResponse?.ErrorCode);
        }
    }
}
