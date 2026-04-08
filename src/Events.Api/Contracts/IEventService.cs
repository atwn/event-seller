namespace Events.Api.Contracts
{
    public interface IEventService
    {
        IEnumerable<Model.Event> GetAll();

        Model.Event? GetEventById(int id);

        void Remove(Model.Event eventItem);

        bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null);

        int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null);

        int GetNextId();
    }
}
