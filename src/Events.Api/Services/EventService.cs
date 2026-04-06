namespace Events.Api.Services
{
    public class EventService : Contracts.IEventService
    {
        private readonly List<Model.Event> _data = [];

        public EventService()
        {
            _data.Add(new Model.Event
            {
                Id = 1,
                Title = "Metallica Concert",
                Description = "Experience the legendary Metallica live in concert!",
                StartAt = DateTime.UtcNow.AddDays(1).Date.AddHours(22), // tomorrow at 10pm UTC
                EndAt = DateTime.UtcNow.AddDays(2).Date.AddHours(1), // the day after tomorrow at 1am UTC
            });
            _data.Add(new Model.Event
            {
                Id = 2,
                Title = "Cirque Du Soleil Show",
                StartAt = DateTime.UtcNow.AddDays(7).Date.AddHours(15), // in a week at 3pm UTC
                EndAt = DateTime.UtcNow.AddDays(7).Date.AddHours(17).AddMinutes(30), // same day at 5:30pm UTC
            });
        }

        public IEnumerable<Model.Event> GetAll()
        {
            return _data;
        }

        public Model.Event? GetEventById(int id)
        {
            return _data.FirstOrDefault(e => e.Id == id);
        }

        public void Remove(Model.Event eventItem)
        {
            _data.Remove(eventItem);
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

        public int GetNextId()
        {
            return _data.Max(e => e.Id) + 1;
        }
    }
}
