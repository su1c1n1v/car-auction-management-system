using CarAuctionManagementSystem.API.ExceptionHandler;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarAuctionManagementSystem.Test.ExceptionHandler;

public class ExceptionHandleMiddlewareTest
{
    public class ExceptionHandleMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly ExceptionHandleMiddleware _middleware;

        public ExceptionHandleMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            Mock<ILogger<ExceptionHandleMiddleware>> loggerMock = new();
            _middleware = new ExceptionHandleMiddleware(_nextMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Invoke_Should_Handle_BadHttpRequestException()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new BadHttpRequestException("Bad Request");

            _nextMock.Setup(x => x(It.IsAny<HttpContext>())).Throws(exception);

            // Act
            await _middleware.Invoke(httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_Should_Handle_ValidationException()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new ValidationException("Validation failed", new List<FluentValidation.Results.ValidationFailure>());

            _nextMock.Setup(x => x(It.IsAny<HttpContext>())).Throws(exception);

            // Act
            await _middleware.Invoke(httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_Should_Handle_NullReferenceException()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new NullReferenceException("Null Reference");

            _nextMock.Setup(x => x(It.IsAny<HttpContext>())).Throws(exception);

            // Act
            await _middleware.Invoke(httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_Should_Handle_General_Exception()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("General Error");

            _nextMock.Setup(x => x(It.IsAny<HttpContext>())).Throws(exception);

            // Act
            await _middleware.Invoke(httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);
        }
    }
}