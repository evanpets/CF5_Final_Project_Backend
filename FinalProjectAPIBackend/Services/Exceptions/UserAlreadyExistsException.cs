namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
