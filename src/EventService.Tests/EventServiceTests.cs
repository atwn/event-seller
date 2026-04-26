
namespace EventService.Tests;

public class EventServiceTests
{
    private readonly Events.Api.Services.EventService _service;

    public EventServiceTests()
    {
        _service = new Events.Api.Services.EventService();
    }

    [Fact]
    public void CreateEvent_ReturnsValidEventId()
    {
        // Arrange
        var @event = new Events.Api.Model.Event
        {
            Title = "Test Event",
            StartAt = DateTime.UtcNow,
            EndAt = DateTime.UtcNow.AddHours(1),
            Description = "This is a test event."
        };

        // Act
        var id = _service.CreateEvent(@event.Title, @event.StartAt, @event.EndAt, @event.Description);

        // Assert
        Assert.True(id > 0);
    }
}
