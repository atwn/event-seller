namespace Events.Api.Services
{
    public class EventService : Contracts.IEventService
    {
        private readonly List<Model.Event> _data = [];

        public EventService()
        {
        }

        public IEnumerable<Model.Event> GetAll()
        {
            return _data;
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
