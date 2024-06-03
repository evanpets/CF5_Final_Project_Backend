using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Services
{
    public interface IPerformerService
    {
        Task<Performer?> FindPerformer(string name);
        Task<List<Performer>> FindAllPerformers();
        Task<List<Performer>> FindAllPerformersWithName(string name);
    }
}
