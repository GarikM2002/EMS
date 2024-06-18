using Dapper;
using DataAccess.Enities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly DataContext dbContext;

    public ContractRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Contract>> GetAllContractsAsync()
    {
        using var connection = dbContext.CreateConnection();
        string sql = "SELECT * FROM Contracts";
        return await connection.QueryAsync<Contract>(sql);
    }

    public async Task<IEnumerable<Contract>> GetContractsByEmployeeIdAsync(int employeeId)
    {
        using var connection = dbContext.CreateConnection();
        string sql = "SELECT * FROM Contracts WHERE EmployeeId = @EmployeeId";
        return await connection.QueryAsync<Contract>(sql, new { EmployeeId = employeeId });
    }

    public async Task<Contract?> GetContractByIdAsync(int id)
    {
        using var connection = dbContext.CreateConnection();
        string sql = "SELECT * FROM Contracts WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Contract>(sql, new { Id = id });
    }

    public async Task<int> CreateContractAsync(Contract contract)
    {
        using var connection = dbContext.CreateConnection();
        string sql = @"
                INSERT INTO Contracts (EmployeeId, ContractType, StartDate, EndDate, Salary)
                VALUES (@EmployeeId, @ContractType, @StartDate, @EndDate, @Salary);
                SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.QuerySingleAsync<int>(sql, contract);
    }

    public async Task<int> UpdateContractAsync(Contract contract)
    {
        using (var connection = dbContext.CreateConnection())
        {
            string sql = @"
                UPDATE Contracts
                SET EmployeeId = @EmployeeId,
                    ContractType = @ContractType,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Salary = @Salary
                WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, contract);
        }
    }

    public async Task<int> DeleteContractAsync(int id)
    {
        using (var connection = dbContext.CreateConnection())
        {
            string sql = "DELETE FROM Contracts WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
