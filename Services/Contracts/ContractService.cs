using AutoMapper;
using DataAccess.Enities;
using DataAccess.Interfaces;
using Shared.DTOs;

namespace Services.Contracts;

public class ContractService(IContractRepository contractRepository, IMapper mapper)
    : IContractService
{
    private readonly IContractRepository contractRepository = contractRepository;
    private readonly IMapper mapper = mapper;

    public async Task<int> CreateContractAsync(ContractViewModel model)
    {
        Contract contract = mapper.Map<ContractViewModel, Contract>(model);

        return await contractRepository.CreateContractAsync(contract);
    }

    public async Task<int> DeleteContractAsync(int id)
    {
        return await contractRepository.DeleteContractAsync(id);
    }

	public async Task<IEnumerable<ContractViewModel>> GetAllBySearchPatternAsync(string pattern)
	{
		var contracts = await contractRepository.GetAllBySearchPatternAsync(pattern);
        return mapper.Map<IEnumerable<Contract>, IEnumerable<ContractViewModel>>(contracts);
	}

	public async Task<IEnumerable<ContractViewModel>> GetAllContractsAsync()
    {
        var contracts = await contractRepository.GetAllContractsAsync();
        return mapper.Map<IEnumerable<Contract>, IEnumerable<ContractViewModel>>(contracts);
    }

    public async Task<ContractViewModel?> GetContractByIdAsync(int id)
    {
        var contract = await contractRepository.GetContractByIdAsync(id);

        return contract is null ? null : mapper.Map<Contract, ContractViewModel>(contract);
    }

    public async Task<IEnumerable<ContractViewModel>> GetContractsByEmployeeEmployersIdAsync(int eeerID)
    {
        var contracts = await contractRepository.GetContractsByEmployerIdAsync(eeerID);

        return mapper.Map<IEnumerable<Contract>, IEnumerable<ContractViewModel>>(contracts);
    }

    public async Task<int> UpdateContractAsync(ContractViewModel model)
    {
        Contract contract = mapper.Map<ContractViewModel, Contract>(model);

        return await contractRepository.UpdateContractAsync(contract);
    }
}
