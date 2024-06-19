using DataAccess.Enities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ContractsController(IContractRepository contractRepository) : ControllerBase
{
    private readonly IContractRepository contractRepository = contractRepository;

    [HttpGet]
    public async Task<IActionResult> GetAllContracts()
    {
        var contracts = await contractRepository.GetAllContractsAsync();
        return Ok(contracts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContractById(int id)
    {
        var contract = await contractRepository.GetContractByIdAsync(id);
        if (contract == null)
        {
            return NotFound();
        }
        return Ok(contract);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetContractsByEmployeeId(int employeeId)
    {
        var contracts = await contractRepository.GetContractsByEmployeeIdAsync(employeeId);
        return Ok(contracts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract(Contract contract)
    {
        var id = await contractRepository.CreateContractAsync(contract);
        contract.Id = id;
        return CreatedAtAction(nameof(GetContractById), new { id = id }, contract);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContract(Contract contract)
    {
        await contractRepository.UpdateContractAsync(contract);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        await contractRepository.DeleteContractAsync(id);
        return NoContent();
    }
}
