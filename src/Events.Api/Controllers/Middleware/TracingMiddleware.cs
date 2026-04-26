using System.Diagnostics;

namespace Events.Api.Controllers.Middleware
{
    /// <summary>
    /// Добавляет трассировку в заголовки ответов и контекст логирования для каждого запроса,
    /// используя TraceId из заголовка "x-request-id" запроса или генерируя новый.
    /// </summary>
    public class TracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TracingMiddleware> _logger;

        public TracingMiddleware(
            RequestDelegate next,
            ILogger<TracingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId =
                context.Request.Headers["x-request-id"].FirstOrDefault()
                ?? Activity.Current?.TraceId.ToString()
                ?? context.TraceIdentifier;

            context.Items["TraceId"] = traceId;
            context.Response.Headers["x-request-id"] = traceId;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["TraceId"] = traceId
            })) {
                await _next(context);
            }
        }
    }
}
