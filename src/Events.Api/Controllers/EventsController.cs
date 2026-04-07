using Events.Api.Contracts;
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
    public ActionResult<IEnumerable<Model.Event>> GetAll()
    {
        var events = _eventService.GetAll();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public ActionResult<Model.Event> GetById([FromRoute] int id)
    {
        var eventItem = _eventService.GetEventById(id);
        if (eventItem == null)
        {
            return NotFound();
        }

        return Ok(eventItem);
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] Dtos.EventDto body)
    {
        var nextId = _eventService.CreateEvent(body.Title, body.StartAt!.Value, body.EndAt!.Value, body.Description);
        var newEvent = _eventService.GetEventById(nextId);
        return CreatedAtAction(nameof(GetById), new { id = nextId }, newEvent);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEvent([FromRoute] int id, [FromBody] Dtos.EventDto body)
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
