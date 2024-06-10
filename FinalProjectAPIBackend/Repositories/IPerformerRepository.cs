using FinalProjectAPIBackend.Data;

namespace FinalProjectAPIBackend.Repositories
{
    public interface IPerformerRepository
    {
        Task<Performer?> GetPerformerAsync(int performerId);
        Task<Performer?> GetPerformerByNameAsync(string name);
        Task<List<Performer>> GetAllPerformersAsync();
        Task<List<Performer>> GetAllPerformersWithNameAsync(string name);
        Task<Performer?> UpdatePerformerInformationAsync(Performer performer);
    }
}
