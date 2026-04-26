namespace Events.Api.Contracts
{
    public record FilterOptions(
        string? Title = null,
        DateTime? From = null,
        DateTime? To = null
    );
}
