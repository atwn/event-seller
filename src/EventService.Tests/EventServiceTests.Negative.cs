namespace EventService.Tests;

public partial class EventServiceTests
{
    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void GetEventById_InvalidId_ThrowsArgumentException()
    {
        // Arrange
        var invalidId = -1;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetEventById(invalidId));
    }

    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void GetEventById_MissingId_ReturnsNull()
    {
        // Arrange
        var missingId = 20;

        // Act & Assert
        Assert.Null(_service.GetEventById(missingId));
    }
    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void UpdateEvent_InvalidId_ThrowsArgumentException()
    {
        // Arrange
        var invalidId = -1;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.TryUpdate(invalidId, "Test Event", DateTime.UtcNow, DateTime.UtcNow.AddHours(1), "This is a test event."));
    }

    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void UpdateEvent_MissingId_ReturnsFalse()
    {
        // Arrange
        var missingId = 20;

        // Act & Assert
        Assert.False(_service.TryUpdate(missingId, "Test Event", DateTime.UtcNow, DateTime.UtcNow.AddHours(1), "This is a test event."));
    }

    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void CreateEvent_InvalidData_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.CreateEvent("Test Event", DateTime.UtcNow.AddHours(1), DateTime.UtcNow, "This is a test event."));
    }

    [Fact]
    [Trait("Target", nameof(Events.Api.Services.EventService))]
    [Trait("Complexity", "Low")]
    [Trait("Direction", "Negative")]
    public void UpdateEvent_InvalidData_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.TryUpdate(1, "Test Event", DateTime.UtcNow.AddHours(1), DateTime.UtcNow, "This is a test event."));
    }
}
