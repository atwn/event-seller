using Events.Api.Contracts;
using Events.Api.Controllers.Dtos;
using Events.Api.Controllers.Filters;
using Events.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// добавить сервисы в DI-контейнер:
builder.Services.AddSingleton<IEventService>(provider =>
{
    var service = new EventService();

    service.Add(new Events.Api.Model.Event
    {
        Id = 1,
        Title = "Metallica Concert",
        Description = "Experience the legendary Metallica live in concert!",
        StartAt = DateTime.UtcNow.AddDays(1).Date.AddHours(22), // tomorrow at 10pm UTC
        EndAt = DateTime.UtcNow.AddDays(2).Date.AddHours(1), // the day after tomorrow at 1am UTC
    });
    service.Add(new Events.Api.Model.Event
    {
        Id = 2,
        Title = "Cirque Du Soleil Show",
        StartAt = DateTime.UtcNow.AddDays(7).Date.AddHours(15), // in a week at 3pm UTC
        EndAt = DateTime.UtcNow.AddDays(7).Date.AddHours(17).AddMinutes(30), // same day at 5:30pm UTC
    });

    return service;
});

// добавить контроллеры в DI-контейнер:
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
                Message = "Проверьте правильность введённых данных.",
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
    // использовать фильтр для перобразования маршрутов в нижний регистр:
    options.DocumentFilter<LowerCasePathFilter>();

    // добавить документацию:
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
