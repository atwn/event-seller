namespace Events.Api.Contracts
{
    public interface IEventService
    {
        PagedResult<Model.Event> GetFilteredEvents(PaginationOptions pagination, FilterOptions? filter = null);

        Model.Event? GetEventById(int id);

        int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryRemove(int id);

        int GetNextId();
    }

    public record PagedResult<T>(IEnumerable<T> Items, int CurrentPage, int TotalPages, int TotalItems);
}
