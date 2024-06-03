namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class PerformerAlreadyExistsException : Exception
    {
        public PerformerAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
