using Events.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public ActionResult<Model.Event> GetById(int id)
    {
        var eventItem = _eventService.GetEventById(id);
        if (eventItem == null)
        {
            return NotFound();
        }

        return Ok(eventItem);
    }
}
