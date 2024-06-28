using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DTOs;

namespace EMS.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ContractsController(IContractService contractService) : ControllerBase
{
	private readonly IContractService contractService = contractService;

	[HttpGet]
	public async Task<IActionResult> GetAllContracts()
	{
		var contracts = await contractService.GetAllContractsAsync();

		return Ok(contracts);
	}

	[HttpGet("all/{page}/{pageSize}")]
	public async Task<IActionResult> GetAllContractsPaginatedAsync(int page, int pageSize)
	{
		var contracts = await contractService.GetAllContractsPaginatedAsync(page, pageSize);

		return Ok(contracts);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetContractById(int id)
	{
		var contract = await contractService.GetContractByIdAsync(id);
		if (contract == null)
		{
			return NotFound();
		}
		return Ok(contract);
	}

	[HttpGet("employee/{employeeEmployersId}")]
	public async Task<IActionResult> GetContractsByEmployeeId(int employeeEmployersId)
	{
		var contracts = await contractService.GetContractsByEmployeeEmployersIdAsync(employeeEmployersId);
		return Ok(contracts);
	}

	[HttpGet("search/{pattern}")]
	public async Task<IActionResult> GetAllContractsBySearchPattern(string pattern)
	{
		var contracts = await contractService.GetAllBySearchPatternAsync(pattern);
		return Ok(contracts);
	}

	[HttpGet("search/{pattern}/{page}/{pageSize}")]
	public async Task<IActionResult> GetAllContractsBySearchPattern(string pattern, int page, int pageSize)
	{
		var contracts = await contractService.GetAllBySearchPatternPaginatedAsync(pattern, page, pageSize);
		return Ok(contracts);
	}

	[HttpPost]
	public async Task<IActionResult> CreateContract(ContractViewModel contract)
	{
		contract.Id = await contractService.CreateContractAsync(contract);

		return CreatedAtAction(nameof(GetContractById), new { contract.Id }, contract);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateContract(ContractViewModel contract)
	{
		await contractService.UpdateContractAsync(contract);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteContract(int id)
	{
		await contractService.DeleteContractAsync(id);
		return NoContent();
	}
}
