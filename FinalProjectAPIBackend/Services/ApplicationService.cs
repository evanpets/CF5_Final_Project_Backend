using AutoMapper;
using FinalProjectAPIBackend.Repositories;
using Microsoft.Extensions.Logging;

namespace FinalProjectAPIBackend.Services
{
    public class ApplicationService : IApplicationService
    {

        protected readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService>? _userLogger;
        private readonly ILogger<EventService>? _eventLogger;
        private readonly ILogger<VenueService>? _venueLogger;
        private readonly ILogger<PerformerService>? _performerLogger;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService>? userLogger,
            ILogger<EventService>? eventLogger, ILogger<VenueService>? venueLogger, ILogger<PerformerService>? performerLogger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userLogger = userLogger;
            _eventLogger = eventLogger;
            _venueLogger = venueLogger;
            _performerLogger = performerLogger;
        }


        public EventService EventService => new(_unitOfWork, _mapper, _eventLogger!);

        public UserService UserService => new(_unitOfWork, _mapper, _userLogger!);

        public VenueService VenueService => new(_unitOfWork, _mapper, _venueLogger!);

        public PerformerService PerformerService => new(_unitOfWork, _mapper, _performerLogger!);
    }
}
