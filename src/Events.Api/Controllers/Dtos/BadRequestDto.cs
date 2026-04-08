namespace Events.Api.Controllers.Dtos
{
    public class BadRequestDto
    {
        public required string Message { get; set; }

        public int Status { get; set; }

        public required IDictionary<string, string[]> Errors { get; set; }
    }
}
