namespace Events.Api.Contracts.Dtos
{
    public record PaginationOptions(
        int Page = 1,
        int PageSize = 10
    );
}
