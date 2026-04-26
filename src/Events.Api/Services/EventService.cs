namespace Events.Api.Services
{
    public class EventService : Contracts.IEventService
    {
        private readonly List<Model.Event> _data = [];

        public EventService()
        {
        }

        public Contracts.PaginatedResult<Model.Event> GetFilteredEvents(Contracts.FilterOptions? filter = null, Contracts.PaginationOptions? pagination = null)
        {
            // применить фильтры к данным, используя отложенное выполнение LINQ:
            IEnumerable<Model.Event> items = _data.OrderBy(e => e.Id);
            if (filter?.Title is string title) {
                items = items.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            if (filter?.From is DateTime from) {
                items = items.Where(e => e.StartAt >= from);
            }
            if (filter?.To is DateTime to) {
                items = items.Where(e => e.EndAt <= to);
            }

            // если не переданы параметры пагинации, использовать значения по умолчанию:
            pagination ??= new Contracts.PaginationOptions();

            // посчитать общее количество элементов и страниц до материализации результата:
            var totalCount = items.Count();
            var totalPages = totalCount == 0 ? 1 : (totalCount + pagination.PageSize - 1) / pagination.PageSize; // округление вверх без использования Math.Ceiling

            // запрошенный номер страницы не должен превышать totalPages:
            if (pagination.Page > totalPages) {
                throw new ArgumentException($"Номер запрошенной страницы {pagination.Page} превышает общее количество страниц {totalPages}.");
            }

            // применить пагинацию и материализовать результат:
            items = items
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList(); // материализовать результат, чтобы избежать повторного выполнения фильтрации

            return new Contracts.PaginatedResult<Model.Event>(Items: items, CurrentPage: pagination.Page, TotalPages: totalPages, TotalItems: totalCount);
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
            return _data.Select(e => e.Id).DefaultIfEmpty(0).Max() + 1;
        }

        public void Add(Model.Event @event)
        {
            _data.Add(@event);
        }
    }
}
