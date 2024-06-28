using Shared.DTOs;

namespace EMS.Services;

public class ContractService(EMSHttpClient httpClient)
{
	private readonly EMSHttpClient httpClient = httpClient;

	public async Task<IEnumerable<ContractViewModel>?> GetAllContractsAsync()
	{
		return await httpClient.GetFromJsonAsync<IEnumerable<ContractViewModel>>("/api/contracts");
	}

	public async Task<ContractViewModel?> GetContractByIdAsync(int id)
	{
		return await httpClient.GetFromJsonAsync<ContractViewModel>($"/api/contracts/{id}");
	}

	public async Task<HttpResponseMessage> CreateContractAsync(ContractViewModel contract)
	{
		return await httpClient.PostAsJsonAsync("/api/contracts", contract);
	}

	public async Task<HttpResponseMessage> UpdateContractAsync(ContractViewModel contract)
	{
		return await httpClient.PutAsJsonAsync($"/api/contracts", contract);
	}

	public async Task<HttpResponseMessage> DeleteContractAsync(int id)
	{
		return await httpClient.DeleteAsync($"/api/contracts/{id}");
	}

	public async Task<IEnumerable<ContractViewModel>?> GetContractsBySearchPatternAsync(string pattern)
	{
		if (string.IsNullOrEmpty(pattern))
			return await GetAllContractsAsync();

		return await httpClient.GetFromJsonAsync<IEnumerable<ContractViewModel>?>(
			$"/api/contracts/search/{pattern}");
	}
}

