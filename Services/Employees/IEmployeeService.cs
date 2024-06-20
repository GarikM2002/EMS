using Shared.DTOs;

namespace Services.Employees;

public interface IEmployeeService
{
    public Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync();
    public Task<EmployeeViewModel?> GetEmployeeByIdAsync(int id);
    public Task<int> CreateEmployeeAsync(EmployeeViewModel employee);
    public Task<int> UpdateEmployeeAsync(EmployeeViewModel employee);
    public Task<int> DeleteEmployeeAsync(int id);
    public Task<bool> ExistsAsync(string email);
}