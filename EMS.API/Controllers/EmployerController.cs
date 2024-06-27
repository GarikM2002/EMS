using DataAccess.Enities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Employees;
using Services.Employers;
using Shared.DTOs;

namespace EMS.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class EmployerController(IEmployerService employerService) : ControllerBase
{
    private readonly IEmployerService employerService = employerService;

    [HttpGet]
    public async Task<IActionResult> GetAllEmployers()
    {
        var employers = await employerService.GetAllEmployersAsync();
        return Ok(employers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployerById(int id)
    {
        var employer = await employerService.GetEmployerByIdAsync(id);
        if (employer == null)
        {
            return NotFound();
        }
        return Ok(employer);
    }

    [HttpGet("by-email")]
    public async Task<IActionResult> GetEmployerByEmail([FromQuery] string email)
    {
        var employer = await employerService.GetEmployerByEmailAsync(email);
        if (employer == null)
        {
            return NotFound();
        }
        return Ok(employer);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployer(EmployerViewModel employer)
    {
        var existingEmployer = await employerService.GetEmployerByIdAsync(employer.Id);
        if (existingEmployer == null)
            return NotFound();

        await employerService.UpdateEmployerAsync(employer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployer(int id)
    {
        var existingEmployer = await employerService.GetEmployerByIdAsync(id);
        if (existingEmployer == null)
        {
            return NotFound();
        }

        await employerService.DeleteEmployerAsync(id);
        return NoContent();
    }
}
