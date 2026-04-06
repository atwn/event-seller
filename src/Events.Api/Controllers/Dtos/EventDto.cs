namespace Events.Api.Controllers.Dtos
{
    public class EventDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
