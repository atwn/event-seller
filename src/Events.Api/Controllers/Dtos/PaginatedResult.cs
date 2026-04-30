namespace Events.Api.Controllers.Dtos
{
    /// <summary>
    /// Контейнер для постраничной передачи данных
    /// </summary>
    /// <typeparam name="T">Тип данных передаваемых в <see cref="Items"/></typeparam>
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public required int CurrentPage { get; set; }
        public required int TotalPages { get; set; }
        public required int TotalItems { get; set; }
    }
}
