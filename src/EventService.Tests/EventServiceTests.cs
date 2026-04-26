namespace EventService.Tests;

public class EventServiceTests
{
    private readonly Events.Api.Services.EventService _service;

    public EventServiceTests()
    {
        _service = new Events.Api.Services.EventService();
    }

    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
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

    [Theory]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Medium")]
    [MemberData(nameof(EventServiceTestData.UpToTwoEvents), MemberType = typeof(EventServiceTestData))]
    public void GetAllEvents_ReturnsAllEvents(List<Events.Api.Model.Event> storedEvents)
    {
        // Arrange
        foreach (var @event in storedEvents) {
            _service.Add(@event);
        }

        // Act
        var result = _service.GetFilteredEvents();

        // Assert
        Assert.Equal(storedEvents, result.Items);
    }

    [Theory]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Medium")]
    [MemberData(nameof(EventServiceTestData.GetEventByIdData), MemberType = typeof(EventServiceTestData))]
    public void GetEventById_EventExists_ReturnsCorrectEvent(List<Events.Api.Model.Event> storedEvents, int eventId, Events.Api.Model.Event expectedEvent)
    {
        // Arrange
        foreach (var @event in storedEvents) {
            _service.Add(@event);
        }

        // Act
        var result = _service.GetEventById(eventId);

        // Assert
        Assert.Equal(expectedEvent, result);
    }
}
