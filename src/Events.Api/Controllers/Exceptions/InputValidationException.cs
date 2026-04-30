namespace Events.Api.Controllers.Exceptions
{
    public class InputValidationException : Contracts.Exceptions.DomainException
    {
        public InputValidationException() : base() { }

        public InputValidationException(string? message) : base(message) { }

        public InputValidationException(string? message, Exception? innerException) : base(message, innerException) { }

        public required Dictionary<string, string[]> Errors { get; set; }
    }
}
