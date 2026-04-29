using Events.Api.Contracts;
using Events.Api.Model;

namespace EventService.Tests;

public static class EventServiceTestData
{
    private static readonly Dictionary<string, Event> _events = new Dictionary<string, Event>
    {
        ["Metallica"] = new Event
        {
            Id = 1,
            Title = "Metallica Concert",
            StartAt = DateTime.UtcNow.AddDays(7),
            EndAt = DateTime.UtcNow.AddDays(7).AddHours(3),
            Description = "Metallica live in concert!"
        },
        ["Coldplay"] = new Event
        {
            Id = 2,
            Title = "Coldplay Concert",
            StartAt = DateTime.UtcNow.AddDays(14),
            EndAt = DateTime.UtcNow.AddDays(14).AddHours(3),
            Description = "Coldplay live in concert!"
        },
        ["Football Match"] = new Event
        {
            Id = 3,
            Title = "Football Match",
            StartAt = DateTime.UtcNow.AddDays(3),
            EndAt = DateTime.UtcNow.AddDays(3).AddHours(2),
            Description = "Exciting football match between top teams!"
        },
        ["Guided Tour"] = new Event
        {
            Id = 4,
            Title = "Guided Tour",
            StartAt = DateTime.UtcNow.AddDays(7),
            EndAt = DateTime.UtcNow.AddDays(7).AddHours(1),
            Description = "Explore the city's landmarks with us!"
        },
        ["Sightseeing Tour"] = new Event
        {
            Id = 5,
            Title = "Sightseeing Tour",
            StartAt = DateTime.UtcNow.AddDays(1),
            EndAt = DateTime.UtcNow.AddDays(1).AddHours(4),
            Description = "Explore the city's landmarks with us!"
        },
        ["Tech Conference"] = new Event
        {
            Id = 6,
            Title = "Tech Conference",
            StartAt = DateTime.UtcNow.AddDays(30),
            EndAt = DateTime.UtcNow.AddDays(30).AddHours(8),
            Description = "Join industry leaders at our annual tech conference!"
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

    public static TheoryData<List<Event>, FilterOptions, List<Event>> GetFilteredEventsData => new()
    {
        { new List<Event>(), new FilterOptions(Title: "Concert"), new List<Event>() },
        { _events.Values.ToList(), new FilterOptions(Title: "CONCERT"), new List<Event> { _events["Metallica"], _events["Coldplay"] } },
        { _events.Values.ToList(), new FilterOptions(Title: "Automobile"), new List<Event>() },
        { _events.Values.ToList(), new FilterOptions(From: DateTime.UtcNow.AddDays(8)), new List<Event> { _events["Coldplay"], _events["Tech Conference"] } },
        { _events.Values.ToList(), new FilterOptions(To: DateTime.UtcNow.AddDays(5)), new List<Event> { _events["Football Match"], _events["Sightseeing Tour"] } },
        { _events.Values.ToList(), new FilterOptions(From: DateTime.UtcNow.AddDays(5), To: DateTime.UtcNow.AddDays(8)), new List<Event> { _events["Metallica"], _events["Guided Tour"] } },
        { _events.Values.ToList(), new FilterOptions(Title: "con", From: DateTime.UtcNow.AddDays(5), To: DateTime.UtcNow.AddDays(8)), new List<Event> { _events["Metallica"] } },
    };

    public static TheoryData<List<Event>, PaginationOptions, PaginatedResult<Event>> GetEventsWithPaginationData => new()
    {
        { new List<Event>(), new PaginationOptions(Page: 1, PageSize: 2), new PaginatedResult<Event>(new List<Event>(), 1, 1, 0) },
        { _events.Values.ToList(), new PaginationOptions(Page: 1, PageSize: 2), new PaginatedResult<Event>(new List<Event> { _events["Metallica"], _events["Coldplay"] }, 1, 3, 6) },
        { _events.Values.ToList(), new PaginationOptions(Page: 2, PageSize: 2), new PaginatedResult<Event>(new List<Event> { _events["Football Match"], _events["Guided Tour"] }, 2, 3, 6) },
        { _events.Values.ToList(), new PaginationOptions(Page: 3, PageSize: 2), new PaginatedResult<Event>(new List<Event> { _events["Sightseeing Tour"], _events["Tech Conference"] }, 3, 3, 6) },
        { _events.Values.ToList(), new PaginationOptions(Page: 2, PageSize: 5), new PaginatedResult<Event>(new List<Event> { _events["Tech Conference"] }, 2, 2, 6) },
    };

    public static TheoryData<List<Event>, FilterOptions, PaginationOptions, PaginatedResult<Event>> GetFilteredEventsWithPaginationData => new()
    {
        { new List<Event>(), new FilterOptions(Title: "Concert"), new PaginationOptions(Page: 1, PageSize: 2), new PaginatedResult<Event>(Items: [], CurrentPage: 1, TotalPages: 1, TotalItems: 0) },
        { _events.Values.ToList(), new FilterOptions(From: DateTime.UtcNow.AddDays(5), To: DateTime.UtcNow.AddDays(8)), new PaginationOptions(Page: 1, PageSize: 2), new PaginatedResult<Event>(Items: new List<Event> { _events["Metallica"], _events["Guided Tour"] }, 1, 1, 2) },
        { _events.Values.ToList(), new FilterOptions(Title: "t", From: DateTime.UtcNow.AddDays(2), To: DateTime.UtcNow.AddDays(45)), new PaginationOptions(Page: 2, PageSize: 3), new PaginatedResult<Event>(Items: new List<Event> { _events["Guided Tour"], _events["Tech Conference"] }, 2, 2, 5) },
    };
}
