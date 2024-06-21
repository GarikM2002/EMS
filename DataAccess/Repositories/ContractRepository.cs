﻿using Dapper;
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

    public async Task<IEnumerable<Contract>> GetContractsByEmployerIdAsync(int employerId)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"
            SELECT c.* FROM Employers e
            INNER JOIN EmployeeEmployers ee ON e.Id = ee.EmployerId
            INNER JOIN Contracts c ON ee.Id = c.EmployeeEmployersId
            WHERE e.ID = @EmployerId";

        return await connection.QueryAsync<Contract>(sql, new { EmployerId = employerId });
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
                INSERT INTO Contracts (EmployeeEmployersId, ContractType, StartDate, EndDate, Salary, Description)
                VALUES (@EmployeeEmployersId, @ContractType, @StartDate, @EndDate, @Salary, @Description);
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
                    Salary = @Salary,
                    Description = @Description
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
