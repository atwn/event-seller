using Events.Api.Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
            catch (InputValidationException ex)
            {
                // тот же формат ответа, что и в обработчике ошибок валидации (см. ConfigureApiBehaviorOptions(options => {...})),
                // только здесь обрабатываются ещё и исключения, возникшие после предварительной валидации, на этапе выполнения бизнес-логики
                var statusCode = MapToStatusCode(ex);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var errorResponse = new ValidationProblemDetails(ex.Errors)
                {
                    Title = MapToTitle(ex),
                    Status = statusCode,
                    Detail = "Проверьте правильность введённых данных.",
                    Type = "https://tools.ietf.org/html/rfc7807",
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (NotFoundException ex)
            {
                var statusCode = StatusCodes.Status404NotFound;
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var errorResponse = new ProblemDetails
                {
                    Title = MapToTitle(ex),
                    Status = statusCode,
                    Detail = ex.Message,
                    Type = "https://tools.ietf.org/html/rfc7807",
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
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
                    Title = MapToTitle(ex),
                    Status = statusCode,
                    Detail = $"Номер запроса: '{context.Items["TraceId"]}'", // вместо того, чтобы показывать детали системных ошибок
                    Type = "https://tools.ietf.org/html/rfc7807",
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static int MapToStatusCode(Exception ex) => ex switch
        {
            //ArgumentException => StatusCodes.Status400BadRequest,     // на данном этапе это скорее непредвиденная системная ошибка, чем ошибка валидации
            //KeyNotFoundException => StatusCodes.Status404NotFound,    // на данном этапе это скорее системная ошибка,
                                                                        // потому что не понятно где она возникла - при обращении к запрашиваемому ресурсу или где-то ещё
            InputValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        private static string MapToTitle(Exception ex) => ex switch
        {
            InputValidationException => "Ошибка валидации",
            NotFoundException => "Запрашиваемый ресурс не найден",
            _ => "Произошла непредвиденная ошибка при обработке запроса",
        };
    }
}
