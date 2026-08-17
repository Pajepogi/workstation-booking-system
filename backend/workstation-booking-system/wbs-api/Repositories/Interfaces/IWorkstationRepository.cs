using wbs_api.DTOs;
using wbs_api.Models;

namespace wbs_api.Repositories.Interfaces
{
    public interface IWorkstationRepository
    {
        Task<IEnumerable<Workstation>> GetAllAsync();
        Task<IEnumerable<Workstation>> GetByName(string name);
        Task<Workstation?> GetByIdAsync(int id);
        Task<Workstation> CreateAsync(Workstation workstation);
        Task UpdateAsync(Workstation workstation);
        Task DeleteAsync(Workstation workstation);
        Task<IEnumerable<WorkstationStatusDto>> GetWorkstationStatusesByDateAsync(DateTime date, string name);
    }
}
