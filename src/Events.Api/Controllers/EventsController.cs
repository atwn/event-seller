using Events.Api.Contracts;
using Events.Api.Contracts.Dtos;
using Events.Api.Contracts.Exceptions;
using Events.Api.Controllers.Dtos;
using Events.Api.Controllers.Mappers;
using Events.Api.Controllers.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Получить полный список событий
    /// </summary>
    /// <param name="title">(Опциональный) Фильтр событий по названию</param>
    /// <param name="from">(Опциональный) Фильтр событий по дате начала</param>
    /// <param name="to">(Опциональный) Фильтр событий по дате окончания</param>
    /// <param name="page">(Опциональный) Номер страницы для пагинации</param>
    /// <param name="pageSize">(Опциональный) Количество элементов на странице</param>
    /// <response code="200">Возвращает полный список зарегистрированных событий</response>
    /// <response code="400">Ошибка валидации входных данных</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dtos.PaginatedResult<EventResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public ActionResult<Dtos.PaginatedResult<EventResponseDto>> GetAll(
        [FromQuery, PositiveInteger] int page = 1,
        [FromQuery, PositiveInteger] int pageSize = 10,
        [FromQuery] string? title = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var filter = new FilterOptions(title, from, to);
        var pagination = new PaginationOptions(page, pageSize);
        try
        {
            var currentPage = _eventService.GetFilteredEvents(filter, pagination);
            var responseBody = new Dtos.PaginatedResult<EventResponseDto>
            {
                Items = [.. currentPage.Items.Select(Map.ToResponse)],
                CurrentPage = currentPage.CurrentPage,
                TotalPages = currentPage.TotalPages,
                TotalItems = currentPage.TotalItems
            };

            return Ok(responseBody);
        }
        catch (PaginationException ex)
        {
            // преобразовать доменное исключение в стандартное исключение API-слоя и указать на конкретный параметр, вызвавший ошибку:
            throw new Exceptions.InputValidationException(ex.Message, ex)
            {
                Errors = new Dictionary<string, string[]>
                {
                    [nameof(page)] = [ex.Message]
                }
            };
        }
    }

    /// <summary>
    /// Получить событие по номеру
    /// </summary>
    /// <param name="id">Порядковый номер события</param>
    /// <response code="200">Возвращает событие с заданным порядковым номером</response>
    /// <response code="400">Порядковый номер события задан неверно</response>
    /// <response code="404">Событие не найдено</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public ActionResult<EventResponseDto> GetById([FromRoute, PositiveInteger] int id)
    {
        var @event = _eventService.GetEventById(id);
        if (@event == null)
        {
            throw new Exceptions.NotFoundException($"Событие под номером {id} не найдено.");
        }

        return Ok(Map.ToResponse(@event));
    }

    /// <summary>
    /// Зарегистрировать событие
    /// </summary>
    /// <param name="body">Параметры создаваемого события</param>
    /// <response code="201">Событие успешно зарегистировано</response>
    /// <response code="400">Параметры события заданы неверно</response>
    /// <response code="500">Произошла непредвиденная ошибка при выполнении запроса</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public IActionResult CreateEvent([FromBody] EventCreateDto body)
    {
        var nextId = _eventService.CreateEvent(body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description);
        var @event = _eventService.GetEventById(nextId) ?? throw new ApplicationException("Не удалось получить детали только что созданного события");
        return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = nextId },
                value: Map.ToResponse(@event));
    }

    /// <summary>
    /// Изменить зарегистрированное событие
    /// </summary>
    /// <param name="id">Порядковый номер события</param>
    /// <param name="body">Новые параметры события</param>
    /// <response code="204">Событие успешно зарегистировано</response>
    /// <response code="400">Порядковый номер или параметры события заданы неверно</response>
    /// <response code="404">Событие не найдено</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public IActionResult UpdateEvent([FromRoute, PositiveInteger] int id, [FromBody] EventCreateDto body)
    {
        if (!_eventService.TryUpdate(id, body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description))
        {
            throw new Exceptions.NotFoundException($"Событие под номером {id} не найдено.");
        }

        return NoContent();
    }

    /// <summary>
    /// Удалить зарегистрированное событие
    /// </summary>
    /// <param name="id">Порядковый номер события</param>
    /// <response code="204">Событие успешно удалено</response>
    /// <response code="400">Порядковый номер события задан неверно</response>
    /// <response code="404">Событие не найдено</response>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public IActionResult DeleteEvent([FromRoute, PositiveInteger] int id)
    {
        return _eventService.TryRemove(id) ? NoContent() : throw new Exceptions.NotFoundException($"Событие под номером {id} не найдено.");
    }
}
