using System.Data.SqlClient;
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

	public async Task<IEnumerable<Contract>> GetAllContractsPaginatedAsync(int page, int pageSize)
	{
		using var connection = dbContext.CreateConnection();

		string sql = @"SELECT * FROM Contracts
						ORDER BY Id
						OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
		return await connection.QueryAsync<Contract>(sql, new
		{
			Offset = page * pageSize,
			PageSize = pageSize,
		});
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

	public async Task<IEnumerable<Contract>> GetAllBySearchPatternAsync(string pattern)
	{
		if (string.IsNullOrEmpty(pattern))
		{
			throw new ArgumentException("Search pattern cannot be null or empty", nameof(pattern));
		}

		const string sqlQuery = @"
            SELECT *
			FROM Contracts c
			WHERE 
			  CONVERT(NVARCHAR, ContractTypeId) LIKE @Pattern OR
			  Description LIKE @Pattern OR
			  Format(StartDate, 'MM-dd-yyyy') LIKE @Pattern OR
			  Format(EndDate, 'MM-dd-yyyy') LIKE @Pattern OR
			  CONVERT(NVARCHAR, Salary) LIKE @Pattern OR
			  CONVERT(NVARCHAR, EmployeeEmployersId) LIKE @Pattern;";

		using var connection = dbContext.CreateConnection();

		var results = await connection.QueryAsync<Contract>(sqlQuery, new { Pattern = $"%{pattern}%" });
		return results;
	}

	public async Task<IEnumerable<Contract>> GetAllBySearchPatternPaginatedAsync(string pattern,
		int page, int pageSize)
	{
		if (string.IsNullOrEmpty(pattern))
		{
			throw new ArgumentException("Search pattern cannot be null or empty", nameof(pattern));
		}

		const string sqlQuery = @"
        DECLARE @Pattern NVARCHAR(50);
        SET @Pattern = @PatternParam;

        SELECT * 
        FROM Contracts c
        WHERE 
            CONVERT(NVARCHAR, ContractTypeId) LIKE @Pattern OR
            c.Description LIKE @Pattern OR
            REPLACE(FORMAT(StartDate, 'MM-dd-yyyy'), '-', '') LIKE @Pattern OR
            REPLACE(FORMAT(EndDate, 'MM-dd-yyyy'), '-', '') LIKE @Pattern OR
            CONVERT(NVARCHAR, Salary) LIKE @Pattern OR
            CONVERT(NVARCHAR, EmployeeEmployersId) LIKE @Pattern
        ORDER BY c.Id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
		";

		var parameters = new
		{
			PatternParam = $"%{pattern}%",
			Offset = page * pageSize,
			PageSize = pageSize
		};

		using var connection = dbContext.CreateConnection();

		var results = await connection.QueryAsync<Contract>(sqlQuery, parameters);
		return results;
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
                INSERT INTO Contracts (EmployeeEmployersId, ContractTypeId, StartDate, EndDate, Salary, Description)
                VALUES (@EmployeeEmployersId, @ContractTypeId, @StartDate, @EndDate, @Salary, @Description);
                SELECT CAST(SCOPE_IDENTITY() as int)";
		return await connection.QuerySingleAsync<int>(sql, contract);
	}

	public async Task<int> UpdateContractAsync(Contract contract)
	{
		using var connection = dbContext.CreateConnection();

		string sql = @"
                UPDATE Contracts
                SET EmployeeEmployersId = @EmployeeEmployersId,
                    ContractTypeId = @ContractTypeId,
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
