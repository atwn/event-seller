using Events.Api.Model;

namespace EventService.Tests;

public static class EventServiceTestData
{
    private static readonly Dictionary<string, Events.Api.Model.Event> _events = new Dictionary<string, Event>
    {
        ["Metallica"] = new Events.Api.Model.Event
        {
            Id = 1,
            Title = "Metallica Concert",
            StartAt = DateTime.UtcNow.AddDays(7),
            EndAt = DateTime.UtcNow.AddDays(7).AddHours(3),
            Description = "Metallica live in concert!"
        },
        ["Coldplay"] = new Events.Api.Model.Event
        {
            Id = 2,
            Title = "Coldplay Concert",
            StartAt = DateTime.UtcNow.AddDays(14),
            EndAt = DateTime.UtcNow.AddDays(14).AddHours(3),
            Description = "Coldplay live in concert!"
        }
    };

    public static TheoryData<List<Event>> UpToTwoEvents => new()
    {
        { new List<Event>() },
        { new List<Event> { _events["Metallica"], _events["Coldplay"] } }
    };

    public static TheoryData<List<Event>, int, Event> GetEventByIdData => new()
    {
        { new List<Event> { _events["Metallica"], _events["Coldplay"] }, 1, _events["Metallica"] },
        { new List<Event> { _events["Metallica"], _events["Coldplay"] }, 2, _events["Coldplay"] }
    };
}
