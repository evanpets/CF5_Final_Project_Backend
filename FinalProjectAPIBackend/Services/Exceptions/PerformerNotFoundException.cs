namespace FinalProjectAPIBackend.Services.Exceptions
{
    public class PerformerNotFoundException : Exception
    {
        public PerformerNotFoundException(string? message) : base(message)
        {
        }
    }
}
