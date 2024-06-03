namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string? message) : base(message)
        {
        }
    }
}
