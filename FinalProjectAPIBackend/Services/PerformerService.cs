using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.Repositories;

namespace FinalProjectAPIBackend.Services
{
    public class PerformerService : IPerformerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PerformerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Performer?> FindPerformer(string name)
        {
            return await _unitOfWork.PerformerRepository.GetPerformerByNameAsync(name);
        }

        public async Task<List<Performer>> FindAllPerformers()
        {
            return await _unitOfWork.PerformerRepository.GetAllPerformersAsync();
        }

        public async Task<List<Performer>> FindAllPerformersWithName(string name)
        {
            return await _unitOfWork.PerformerRepository.GetAllPerformersWithNameAsync(name);
        }
    }
}
