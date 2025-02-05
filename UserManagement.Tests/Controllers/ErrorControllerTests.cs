using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.Controllers;

namespace UserManagement.Tests.Controllers
{
    /// <summary>
    /// Represents the unit tests for the <see cref="ErrorController"/> class.
    /// </summary>
    public class ErrorControllerTests
    {
        /// <summary>
        /// Mock object for the ILogger used by the ErrorController.
        /// </summary>
        private readonly Mock<ILogger<ErrorController>> _loggerMock;

        /// <summary>
        /// Instance of the ErrorController being tested.
        /// </summary>
        private readonly ErrorController _errorController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorControllerTests"/> class.
        /// This constructor sets up the necessary mocks and creates the ErrorController instance.
        /// </summary>
        public ErrorControllerTests()
        {
            // Mock the ILogger
            _loggerMock = new Mock<ILogger<ErrorController>>();

            // Create the ErrorController instance using the mocked ILogger
            _errorController = new ErrorController(_loggerMock.Object);
        }

        /// <summary>
        /// Tests if the Error method logs the error details when an exception occurs.
        /// </summary>
        [Fact]
        public void Error_ShouldLogErrorDetails_WhenExceptionOccurs()
        {
            // Arrange
            Mock<HttpContext> contextMock = new Mock<HttpContext>(); // Mock the HttpContext
            Mock<IExceptionHandlerFeature> exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>(); // Mock the IExceptionHandlerFeature
            exceptionHandlerFeatureMock.Setup(x => x.Error).Returns(new Exception("Test exception")); // Setup the exception

            Mock<IFeatureCollection> featureCollectionMock = new Mock<IFeatureCollection>(); // Mock the IFeatureCollection
            featureCollectionMock.Setup(x => x.Get<IExceptionHandlerFeature>()).Returns(exceptionHandlerFeatureMock.Object); // Setup the feature collection
            contextMock.Setup(x => x.Features).Returns(featureCollectionMock.Object); // Setup the context features

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object // Set the controller context HttpContext
            };
            _errorController.ControllerContext = controllerContext; // Set the controller context

            // Act
            IActionResult result = _errorController.Error(); // Call the Error method

            // Assert
            var logErrorCalls = _loggerMock.Invocations
                .Where(invocation => invocation.Method.Name == "Log" &&
                                     invocation.Arguments.Count > 0 &&
                                     invocation.Arguments[2].ToString() == "UNHANDLED EXCEPTION OCCURRED AND IS BEING LOGGED AS ERROR");
            Assert.Single(logErrorCalls); // Assert that the error log message was called once

            Assert.IsType<ObjectResult>(result); // Assert that the result is of type ObjectResult
            ObjectResult? objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult?.StatusCode); // Assert that the status code is 500
        }

        /// <summary>
        /// Tests if the Error method returns ProblemDetails when an exception occurs.
        /// </summary>
        [Fact]
        public void Error_ShouldReturnProblemDetails_WhenExceptionOccurs()
        {
            // Arrange
            Mock<HttpContext> contextMock = new Mock<HttpContext>(); // Mock the HttpContext
            Mock<IExceptionHandlerFeature> exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>(); // Mock the IExceptionHandlerFeature
            exceptionHandlerFeatureMock.Setup(x => x.Error).Returns(new Exception("Test exception")); // Setup the exception

            Mock<IFeatureCollection> featureCollectionMock = new Mock<IFeatureCollection>(); // Mock the IFeatureCollection
            featureCollectionMock.Setup(x => x.Get<IExceptionHandlerFeature>()).Returns(exceptionHandlerFeatureMock.Object); // Setup the feature collection
            contextMock.Setup(x => x.Features).Returns(featureCollectionMock.Object); // Setup the context features

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object // Set the controller context HttpContext
            };
            _errorController.ControllerContext = controllerContext; // Set the controller context

            // Act
            IActionResult result = _errorController.Error(); // Call the Error method

            // Assert
            Assert.IsType<ObjectResult>(result); // Assert that the result is of type ObjectResult
            ObjectResult? objectResult = result as ObjectResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult?.StatusCode); // Assert that the status code is 500
            Assert.IsType<ProblemDetails>(objectResult?.Value); // Assert that the result value is of type ProblemDetails
        }
    }
}