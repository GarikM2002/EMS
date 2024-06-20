using AutoMapper;
using DataAccess.Enities;
using DataAccess.Interfaces;
using Shared.DTOs;

namespace Services.Employees;

public class EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository = employeeRepository;
    private readonly IMapper mapper = mapper;

    public async Task<int> CreateEmployeeAsync(EmployeeViewModel model)
    {
        Employee employee = mapper.Map<EmployeeViewModel, Employee>(model);

        return await employeeRepository.CreateEmployeeAsync(employee);
    }

    public async Task<int> DeleteEmployeeAsync(int id)
    {
        return await employeeRepository.DeleteEmployeeAsync(id);
    }

    public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync()
    {
        var employees = await employeeRepository.GetAllEmployeesAsync();
        return mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
    }

    public async Task<EmployeeViewModel?> GetEmployeeByIdAsync(int id)
    {
        var employee = await employeeRepository.GetEmployeeByIdAsync(id);

        return employee is null ? null : mapper.Map<Employee, EmployeeViewModel>(employee);
    }

    public async Task<int> UpdateEmployeeAsync(EmployeeViewModel model)
    {
        Employee employee = mapper.Map<EmployeeViewModel, Employee>(model);

        return await employeeRepository.UpdateEmployeeAsync(employee);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return (await employeeRepository.GetEmployeeByEmailAsync(email)) != null;
    }
}
