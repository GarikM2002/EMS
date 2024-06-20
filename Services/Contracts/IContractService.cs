using Shared.DTOs;

namespace Services.Contracts;

public interface IContractService
{
    public Task<IEnumerable<ContractViewModel>> GetAllContractsAsync();
    public Task<IEnumerable<ContractViewModel>> GetContractsByEmployeeEmployersIdAsync(int employeeId);
    public Task<ContractViewModel?> GetContractByIdAsync(int id);
    public Task<int> CreateContractAsync(ContractViewModel contract);
    public Task<int> UpdateContractAsync(ContractViewModel contract);
    public Task<int> DeleteContractAsync(int id);
}
