using DataAccess.Enities;

namespace DataAccess.Interfaces
{
    public interface IContractRepository
    {
        public Task<IEnumerable<Contract>> GetAllContractsAsync();
        public Task<IEnumerable<Contract>> GetContractsByEmployerIdAsync(int employerId);
        public Task<Contract?> GetContractByIdAsync(int id);
        public Task<int> CreateContractAsync(Contract contract);
        public Task<int> UpdateContractAsync(Contract contract);
        public Task<int> DeleteContractAsync(int id);
    }
}
