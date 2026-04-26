using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    // Если ответ уже начался, мы не можем изменить его, поэтому просто завершаем обработку
                    return;
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new ProblemDetails
                {
                    Title = "Произошла внутренняя ошибка сервера",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
