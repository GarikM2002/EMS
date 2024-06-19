using DataAccess.Enities;

namespace DataAccess.Interfaces
{
    public interface IEmployerRepository
    {
        public Task<IEnumerable<Employer>> GetAllEmployersAsync();
        public Task<Employer?> GetEmployerByEmailAsync(string email);
        public Task<Employer?> GetEmployerByIdAsync(int id);
        public Task<int> CreateEmployerAsync(Employer employee);
        public Task<int> UpdateEmployerAsync(Employer employee);
        public Task<int> DeleteEmployerAsync(int id);
    }
}
