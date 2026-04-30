using Events.Api.Contracts.Dtos;

namespace Events.Api.Contracts
{
    public interface IEventService
    {
        /// <summary>
        /// Осуществляет выборку событий по заданным параметрам
        /// </summary>
        /// <param name="filter">Параметры фильтрации</param>
        /// <param name="pagination">Параметры пагинации</param>
        /// <exception cref="Exceptions.PaginationException">Указывает на то, что параметры пагинации указаны неверно</exception>
        /// <returns>Страница событий, удовлетворяющих условиям фильтрации</returns>
        PaginatedResult<Model.Event> GetFilteredEvents(FilterOptions? filter = null, PaginationOptions? pagination = null);

        Model.Event? GetEventById(int id);

        int CreateEvent(string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryUpdate(int id, string title, DateTime startAt, DateTime endAt, string? description = null);

        bool TryRemove(int id);

        int GetNextId();
    }
}
