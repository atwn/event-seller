using Events.Api.Controllers.Dtos;

namespace Events.Api.Controllers.Mappers;

public static class Map
{
    public static EventResponseDto ToResponse(Model.Event @event)
    {
        return new EventResponseDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartAt = @event.StartAt,
            EndAt = @event.EndAt
        };
    }
}
