namespace Events.Api.Controllers.Exceptions
{
    public class NotFoundException : Contracts.Exceptions.DomainException
    {
        public NotFoundException() : base() { }

        public NotFoundException(string? message) : base(message) { }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
