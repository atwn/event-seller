using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Events.Api.Controllers.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла необработанная ошибка в запросе {Method} {Path} (TraceId: '{TraceId}'):\n\t{Message}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Items["TraceId"], // TracingMiddleware должен был добавить TraceId в конвейер
                    ex.Message);
                if (context.Response.HasStarted)
                {
                    return;
                }

                var statusCode = MapToStatusCode(ex);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var errorResponse = new ProblemDetails
                {
                    Title = "Произошла внутренняя ошибка сервера",
                    Detail = ex.Message,
                    Status = statusCode,
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static int MapToStatusCode(Exception ex) => ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
