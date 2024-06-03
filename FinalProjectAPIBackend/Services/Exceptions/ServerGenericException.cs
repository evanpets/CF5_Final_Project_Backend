namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class ServerGenericException : Exception
    {
        public ServerGenericException(string? message) : base(message)
        {
        }
    }
}
