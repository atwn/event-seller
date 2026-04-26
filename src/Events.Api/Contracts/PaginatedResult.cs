namespace Events.Api.Contracts
{
    public record PaginatedResult<T>(
        IEnumerable<T> Items,
        int CurrentPage,
        int TotalPages,
        int TotalItems);
}
