using Events.Api.Contracts.Dtos;

namespace Events.Api.Contracts.Exceptions
{
    public class PaginationException : DomainException
    {
        public PaginationException() : base() { }

        public PaginationException(string? message) : base(message) { }

        public PaginationException(string? message, Exception? innerException) : base(message, innerException) { }

        public required PaginationOptions? Options { get; set; }
    }
}
