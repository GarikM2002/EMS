﻿using Dapper;
using DataAccess.Enities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories;
public class EmployeeRepository(DataContext dbContext) : IEmployeeRepository
{
    private readonly DataContext dbContext = dbContext;

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        using var connection = dbContext.CreateConnection();

        string sql = "SELECT * FROM Employees";
        return await connection.QueryAsync<Employee>(sql);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        using var connection = dbContext.CreateConnection();

        string sql = "SELECT * FROM Employees WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
    }

    public async Task<int> CreateEmployeeAsync(Employee employee)
    {
        using var connection = dbContext.CreateConnection();

        string sql = @"SET NOCOUNT ON
            INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, Department)
            VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Department);
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
                Department = @Department
            WHERE Id = @Id"; //Email and Id isn't changed

        return await connection.ExecuteAsync(sql, employee);
    }

    public async Task<int> DeleteEmployeeAsync(int id)
    {
        using var connection = dbContext.CreateConnection();

        string sql = "DELETE FROM Employees WHERE Id = @Id";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        using var connection = dbContext.CreateConnection();

        var query = "SELECT * FROM Employees WHERE Email = @Email";
        var parameters = new { Email = email };

        var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, parameters);
        return employee;
    }
}
