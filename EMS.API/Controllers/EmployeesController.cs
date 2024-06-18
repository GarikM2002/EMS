using DataAccess.Enities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeRepository employeeRepository) : ControllerBase
{
    private readonly IEmployeeRepository employeeRepository = employeeRepository;

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await employeeRepository.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await employeeRepository.GetEmployeeByIdAsync(id);
        if (employee is null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        var id = await employeeRepository.CreateEmployeeAsync(employee);
        employee.Id = id;
        return CreatedAtAction(nameof(GetEmployeeById), new { id = id }, employee);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {        
        await employeeRepository.UpdateEmployeeAsync(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await employeeRepository.DeleteEmployeeAsync(id);
        return NoContent();
    }
}
