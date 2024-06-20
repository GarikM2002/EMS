using Shared.DTOs;

namespace Services.Employers;

public interface IEmployerService
{
    public Task<IEnumerable<EmployerViewModel>> GetAllEmployersAsync();
    public Task<EmployerViewModel?> GetEmployerByEmailAsync(string email);
    public Task<EmployerViewModel?> GetEmployerByIdAsync(int id);
    public Task<int> UpdateEmployerAsync(EmployerViewModel employee);
    public Task<int> DeleteEmployerAsync(int id);
    public Task<bool> ExistsAsync(string email);
}
