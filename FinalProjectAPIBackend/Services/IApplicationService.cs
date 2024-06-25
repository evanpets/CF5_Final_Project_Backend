namespace FinalProjectAPIBackend.Services
{
     /// <summary>
     /// Interface for the application service.
     /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// The event service.
        /// </summary>
        EventService EventService { get; }
        /// <summary>
        /// The user service.
        /// </summary>
        UserService UserService { get; }
        /// <summary>
        /// The venue service.
        /// </summary>
        VenueService VenueService { get; }
        /// <summary>
        /// The performer service.
        /// </summary>
        PerformerService PerformerService { get; }
    }
}
