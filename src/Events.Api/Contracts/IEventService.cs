namespace Events.Api.Contracts
{
    public interface IEventService
    {
        IEnumerable<Model.Event> GetFilteredEvents(FilterOptions? filter);

        Model.Event? GetEventById(int id);

        int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryRemove(int id);

        int GetNextId();
    }
}
