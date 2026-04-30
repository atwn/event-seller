namespace Events.Api.Contracts.Dtos
{
    public record FilterOptions(
        string? Title = null,
        DateTime? From = null,
        DateTime? To = null
    );
}
