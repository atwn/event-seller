using Events.Api.Contracts;
using Events.Api.Controllers.Filters;
using Events.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// register controllers and services in the DI container:
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddControllers();

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
