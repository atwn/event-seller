namespace Events.Api.Contracts
{
    public record PaginationOptions(
        int Page = 1,
        int PageSize = 10
    );
}
