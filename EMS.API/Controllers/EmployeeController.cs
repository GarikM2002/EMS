using DataAccess.Enities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Employees;
using Services.Employers;
using Shared.DTOs;

namespace EMS.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    private readonly IEmployeeService employeeService = employeeService;

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee is null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeViewModel employee)
    {
        if(await employeeService.ExistsAsync(employee.Email))
            return BadRequest("Employee with such an email already exists!");

        employee.Id = await employeeService.CreateEmployeeAsync(employee);

        return CreatedAtAction(nameof(GetEmployeeById), new { employee.Id }, employee);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee(EmployeeViewModel employee)
    {
        var existingEmployee = await employeeService.GetEmployeeByIdAsync(employee.Id);
        if (existingEmployee == null)
            return NotFound();

        await employeeService.UpdateEmployeeAsync(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}
