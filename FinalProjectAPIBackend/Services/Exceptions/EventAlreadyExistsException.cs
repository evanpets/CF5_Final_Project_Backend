namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class EventAlreadyExistsException : Exception
    {
        public EventAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
