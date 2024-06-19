using Dapper;
using DataAccess.Enities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class EmployerRepository(DataContext dbContext) : IEmployerRepository
    {
        private readonly DataContext dbContext = dbContext;

        public async Task<IEnumerable<Employer>> GetAllEmployersAsync()
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = "SELECT * FROM Employers";
            return await connection.QueryAsync<Employer>(sql);
        }

        public async Task<Employer?> GetEmployerByEmailAsync(string email)
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = "SELECT * FROM Employers WHERE Email = @Email";
            return await connection.QuerySingleOrDefaultAsync<Employer>(sql, new { Email = email });
        }

        public async Task<Employer?> GetEmployerByIdAsync(int id)
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = "SELECT * FROM Employers WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Employer>(sql, new { Id = id });
        }

        public async Task<int> CreateEmployerAsync(Employer employer)
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = @"
                INSERT INTO Employers (FirstName, LastName, Email, PhoneNumber, Department, PasswordHash, PasswordSalt)
                VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Department, @PasswordHash, @PasswordSalt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, employer);
        }

        public async Task<int> UpdateEmployerAsync(Employer employer)
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = @"
                UPDATE Employers
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    Department = @Department,
                    PasswordHash = @PasswordHash,
                    PasswordSalt = @PasswordSalt
                WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, employer);
        }

        public async Task<int> DeleteEmployerAsync(int id)
        {
            using var connection = dbContext.CreateConnection();
            
            string sql = "DELETE FROM Employers WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
