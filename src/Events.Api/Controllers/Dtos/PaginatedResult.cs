namespace Events.Api.Controllers.Dtos
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public required int CurrentPage { get; set; }
        public required int TotalPages { get; set; }
        public required int TotalItems { get; set; }
    }
}
