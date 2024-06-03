namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class VenueAlreadyExistsException : Exception
    {
        public VenueAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
