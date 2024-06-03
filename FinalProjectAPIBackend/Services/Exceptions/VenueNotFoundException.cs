namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class VenueNotFoundException : Exception
    {
        public VenueNotFoundException(string? message) : base(message)
        {
        }
    }
}
