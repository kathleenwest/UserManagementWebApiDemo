namespace UserManagement.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to log HTTP request and response details.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request details.
            _logger.LogInformation("Incoming request: {method} {url} {headers}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers.ToString());

            // Capture the original response body stream.
            var originalBodyStream = context.Response.Body;
            using (var responseBodyStream = new MemoryStream())
            {
                // Replace the response body with the new memory stream.
                context.Response.Body = responseBodyStream;

                // Invoke the next middleware in the pipeline.
                await _next(context);

                // Reset the response body stream position to the beginning.
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                // Read the response body stream to the end.
                var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
                // Reset the response body stream position to the beginning again.
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Log the outgoing response details.
                _logger.LogInformation("Outgoing response: {statusCode} {body}",
                    context.Response.StatusCode,
                    responseBody);

                // Copy the content of the new memory stream to the original stream.
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}