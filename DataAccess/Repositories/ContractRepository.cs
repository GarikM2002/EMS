using Dapper;
using DataAccess.Enities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories;

public class ContractRepository(DataContext dbContext) : IContractRepository
{
    private readonly DataContext dbContext = dbContext;

    public async Task<IEnumerable<Contract>> GetAllContractsAsync()
    {
        using var connection = dbContext.CreateConnection();

        string sql = "SELECT * FROM Contracts";
        return await connection.QueryAsync<Contract>(sql);
    }

    public async Task<IEnumerable<Contract>> GetContractsByEmployeeEmployersIdAsync(int employeeEmployersId)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"
            SELECT c.Id, c.ContractType, c.Description, c.StartDate, c.EndDate,
                c.Salary, c.EmployeeEmployersId 
            FROM Contracts c
            INNER JOIN EmployeeEmployers ee ON c.EmployeeEmployersId = ee.Id
            WHERE ee.ID = @EmployeeEmployersId";

        return await connection.QueryAsync<Contract>(sql, new { EmployeeEmployersId = employeeEmployersId });
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
                INSERT INTO Contracts (EmployeeEmployersId, ContractType, StartDate, EndDate, Salary)
                VALUES (@EmployeeEmployersId, @ContractType, @StartDate, @EndDate, @Salary);
                SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.QuerySingleAsync<int>(sql, contract);
    }

    public async Task<int> UpdateContractAsync(Contract contract)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"
                UPDATE Contracts
                SET EmployeeEmployersId = @EmployeeEmployersId,
                    ContractType = @ContractType,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Salary = @Salary
                WHERE Id = @Id";
        return await connection.ExecuteAsync(sql, contract);
    }

    public async Task<int> DeleteContractAsync(int id)
    {
        using var connection = dbContext.CreateConnection();

        string sql = "DELETE FROM Contracts WHERE Id = @Id";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}
