namespace Events.Api.Services
{
    public class EventService : Contracts.IEventService
    {
        private readonly List<Model.Event> _data = [];

        public EventService()
        {
        }

        public IEnumerable<Model.Event> GetFilteredEvents(Contracts.FilterOptions? filter)
        {
            IEnumerable<Model.Event> result = _data;
            if (filter?.Title is string title) {
                result = result.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            if (filter?.From is DateTime from) {
                result = result.Where(e => e.StartAt >= from);
            }
            if (filter?.To is DateTime to) {
                result = result.Where(e => e.EndAt <= to);
            }

            // материализовать результат, чтобы избежать повторного выполнения фильтрации:
            return [.. result];
        }

        public Model.Event? GetEventById(int id)
        {
            return _data.FirstOrDefault(e => e.Id == id);
        }

        public bool TryRemove(int id)
        {
            return GetEventById(id) is Model.Event @event
                && _data.Remove(@event);
        }

        public int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null)
        {
            var nextId = GetNextId();
            var newEvent = new Model.Event
            {
                Id = nextId,
                Title = title,
                StartAt = startAt,
                EndAt = endAt,
                Description = description
            };
            _data.Add(newEvent);
            return nextId;
        }

        public bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null)
        {
            var eventItem = _data.FirstOrDefault(e => e.Id == id);
            if (eventItem == null)
            {
                return false;
            }

            eventItem.Title = title;
            eventItem.StartAt = startAt;
            eventItem.EndAt = endAt;
            eventItem.Description = description;
            return true;
        }

        public int GetNextId()
        {
            return _data.Max(e => e.Id) + 1;
        }

        public void Add(Model.Event @event)
        {
            _data.Add(@event);
        }
    }
}
