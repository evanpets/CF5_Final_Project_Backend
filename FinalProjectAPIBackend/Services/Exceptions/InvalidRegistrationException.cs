namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class InvalidRegistrationException : Exception
    {
        public InvalidRegistrationException(string? message) : base(message)
        {
        }
    }
}
