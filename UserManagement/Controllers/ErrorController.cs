using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    // This controller handles errors and logs the relevant information.
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        // Constructor for ErrorController, which injects an ILogger instance.
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles errors and logs the relevant information.
        /// </summary>
        /// <returns>An IActionResult representing the problem details.</returns>
        [HttpGet("/error")]
        public IActionResult Error()
        {
            // Get the exception details from the current HTTP context.
            IExceptionHandlerFeature? context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context is not null)
            {
                // Extract the stack trace and error message.
                string? stackTrace = context.Error.StackTrace;
                string errorMessage = context.Error.Message;

                // Log the error message and stack trace using ILogger.
                _logger.LogError("UNHANDLED EXCEPTION OCCURRED AND IS BEING LOGGED AS ERROR");
                _logger.LogError("Error: {errorMessage} \n StackTrace: {stackTrace}", errorMessage, stackTrace);
            }

            // Return a generic problem details response.
            return Problem();
        }
    }
}