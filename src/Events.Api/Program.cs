using Events.Api.Contracts;
using Events.Api.Controllers.Dtos;
using Events.Api.Controllers.Filters;
using Events.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// register controllers and services in the DI container:
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // custom validation error handling:
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

            var customResponse = new BadRequestDto
            {
                Message = "Проверьте правильность введенных данных.",
                Status = (int)HttpStatusCode.BadRequest,
                Errors = errors
            };

            return new BadRequestObjectResult(customResponse);
        };
    });

// configure Swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<LowerCasePathFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
