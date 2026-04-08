using Events.Api.Contracts;
using Events.Api.Controllers.Dtos;
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
            .Select(e => new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartAt = e.StartAt,
                EndAt = e.EndAt
            }).ToList();
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

        var eventResponse = new EventResponseDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartAt = @event.StartAt,
            EndAt = @event.EndAt
        };

        return Ok(eventResponse);
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] Dtos.EventCreateDto body)
    {
        var nextId = _eventService.CreateEvent(body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description);
        var newEvent = _eventService.GetEventById(nextId);
        return CreatedAtAction(nameof(GetById), new { id = nextId }, newEvent);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEvent([FromRoute] int id, [FromBody] Dtos.EventCreateDto body)
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
