using System.Data;
using System.Data.SqlClient;
using Dapper;
using DataAccess.Enities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories;
public class EmployeeRepository(DataContext dbContext) : IEmployeeRepository
{
    private readonly DataContext dbContext = dbContext;

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        using var connection = dbContext.CreateConnection();

        string sql = "SELECT * FROM Employees WHERE IsDeleted = 0";
        return await connection.QueryAsync<Employee>(sql);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        using var connection = dbContext.CreateConnection();

        string sql = "SELECT * FROM Employees WHERE Id = @Id and IsDeleted = 0";
        return await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
    }

    public async Task<int> CreateEmployeeAsync(Employee employee)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"SET NOCOUNT ON
            INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, Department, IsDeleted)
            VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Department, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() as int)";

        return await connection.QuerySingleAsync<int>(sql, employee);
    }

    public async Task<int> UpdateEmployeeAsync(Employee employee)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"
            UPDATE Employees
            SET FirstName = @FirstName,
                LastName = @LastName,
                PhoneNumber = @PhoneNumber,
                Department = @Department,
                IsDeleted = @IsDeleted
            WHERE Id = @Id"; //Email and Id isn't changed

        return await connection.ExecuteAsync(sql, employee);
    }

    public async Task<int> DeleteEmployeeAsync(int id)
    {
        using var connection = dbContext.CreateConnection();

        string sql = "UPDATE Employees SET IsDeleted = 1 WHERE Id = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        using var connection = dbContext.CreateConnection();

        var query = "SELECT * FROM Employees WHERE Email = @Email AND IsDeleted = 0";
        var parameters = new { Email = email };

        var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, parameters);
        return employee;
    }

    public async Task<int> CreateOrAppendByEmployerAsync(Employee employee, int employerId)
    {
        int? employeeId = (await GetEmployeeByEmailAsync(employee.Email))?.Id;
        if (employeeId is null)
            employeeId = await CreateEmployeeAsync(employee);

        using var connection = dbContext.CreateConnection();

        if (!await IsAlreadyExistedAsync(employeeId.Value, employerId, connection))
        {
            string sql = "INSERT INTO EmployeeEmployers (EmployeeId, EmployerId) VALUES (@EmployeeId, @EmployerId)";
            await connection.ExecuteAsync(sql, new { EmployeeId = employeeId, EmployerId = employerId });
        }

        return employeeId.Value;
    }

    public static async Task<bool> IsAlreadyExistedAsync(int employeeId, int employerId, IDbConnection connection)
    {
        const string sql = @"
            SELECT COUNT(*)
            FROM EmployeeEmployers
            WHERE EmployeeId = @employeeId AND EmployerId = @employerId;
        ";

        int count = await connection.QuerySingleAsync<int>(sql, new { employeeId, employerId });
        return count > 0;
    }
}
