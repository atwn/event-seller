using Events.Api.Contracts;
using Events.Api.Controllers.Dtos;
using Events.Api.Controllers.Mappers;
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

    [HttpGet]
    public ActionResult<IEnumerable<EventResponseDto>> GetAll()
    {
        var events = _eventService.GetAll()
            .Select(@event => Map.ToResponse(@event))
            .ToList();

        return Ok(events);
    }

    [HttpGet("{id}")]
    public ActionResult<EventResponseDto> GetById([FromRoute] int id)
    {
        var @event = _eventService.GetEventById(id);
        if (@event == null)
        {
            return NotFound();
        }

        return Ok(Map.ToResponse(@event));
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] EventCreateDto body)
    {
        var nextId = _eventService.CreateEvent(body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description);
        var @event = _eventService.GetEventById(nextId);
        return @event != null
            ? CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = nextId },
                value: Map.ToResponse(@event))
            : StatusCode(StatusCodes.Status500InternalServerError); // лучше, наверное, выбросить исключение, и поймать его в middleware,
                                                                    // так как ненайденное событие, в данном случае, - это нештатная ситуация
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEvent([FromRoute] int id, [FromBody] EventCreateDto body)
    {
        if (!_eventService.TryUpdate(id, body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvent([FromRoute] int id)
    {
        var eventItem = _eventService.GetEventById(id);
        if (eventItem == null)
        {
            return NotFound();
        }

        _eventService.Remove(eventItem);
        return NoContent();
    }
}
