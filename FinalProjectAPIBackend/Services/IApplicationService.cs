namespace FinalProjectAPIBackend.Services
{
    public interface IApplicationService
    {
        EventService EventService { get; }
        UserService UserService { get; }
        VenueService VenueService { get; }
        PerformerService PerformerService { get; }
    }
}
