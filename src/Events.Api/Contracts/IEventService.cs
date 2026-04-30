using Events.Api.Contracts.Dtos;

namespace Events.Api.Contracts
{
    public interface IEventService
    {
        PaginatedResult<Model.Event> GetFilteredEvents(FilterOptions? filter = null, PaginationOptions? pagination = null);

        Model.Event? GetEventById(int id);

        int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryRemove(int id);

        int GetNextId();
    }
}
