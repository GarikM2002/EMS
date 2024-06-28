using DataAccess.Enities;

namespace DataAccess.Interfaces
{
	public interface IContractRepository
	{
		Task<IEnumerable<Contract>> GetAllContractsAsync();
		Task<IEnumerable<Contract>> GetAllContractsPaginatedAsync(int page, int pageSize);
		Task<IEnumerable<Contract>> GetContractsByEmployerIdAsync(int employerId);
		Task<IEnumerable<Contract>> GetAllBySearchPatternAsync(string pattern);
		Task<IEnumerable<Contract>> GetAllBySearchPatternPaginatedAsync(string pattern,
		 int page, int pageSize);
		Task<Contract?> GetContractByIdAsync(int id);
		Task<int> CreateContractAsync(Contract contract);
		Task<int> UpdateContractAsync(Contract contract);
		Task<int> DeleteContractAsync(int id);
	}
}
