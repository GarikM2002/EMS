using Dapper;
using DataAccess.Enities;
using DataAccess.Repositories;
using DataAccess;
using DataAccess.Interfaces;

namespace Tests.DataAccess;

public class EmployeeRepositoryTests : IDisposable
{
    private readonly SqlServerDatabaseFixture fixture;
    private readonly DataContext context;
    private readonly IEmployeeRepository repository;

    public EmployeeRepositoryTests()
    {
        fixture = new SqlServerDatabaseFixture();
        context = fixture.DataContext;
        repository = new EmployeeRepository(context);
    }

    [Theory]
    [InlineData(2)]
    public async Task GetAllEmployeesAsync_ReturnsAllEmployees(int expectedCount)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = (await repository.GetAllEmployeesAsync())?.ToArray();

        Employee[] expected = (await connection.QueryAsync<Employee>
            ("SELECT * FROM Employees Where isdeleted = 0")).ToArray();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Length);

        for (int i = 0; i < expected.Length; i++)
        {
            AssertHelper.AssertEmployeesAreEqual(expected[i], result[i]);
        }
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    public async Task GetEmployeeByIdAsync_ReturnsEmployee(int id, bool isNull)
    {
        // Arrange       
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.GetEmployeeByIdAsync(id);
        Employee? employee = await connection.QuerySingleOrDefaultAsync<Employee?>(
            "Select * from employees where Id = @Id AND isdeleted = 0", new { Id = id });

        // Assert
        if (isNull)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        AssertHelper.AssertEmployeesAreEqual(employee!, result);

    }

    [Fact]
    public async Task CreateEmployeeAsync_ReturnsNewEmployeeId()
    {
        // Arrange
        var expected = new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doenew@example.com",
            PhoneNumber = "1234567890",
            Department = "HR"
        };
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.CreateEmployeeAsync(expected);

        expected.Id = result;
        var actual = await connection.QuerySingleAsync<Employee>(
            "SELECT * FROM Employees WHERE Id = @Id", new { Id = result });

        // Assert
        Assert.True(result > 0);
        AssertHelper.AssertEmployeesAreEqual(expected, actual);
    }

    [Theory]
    [InlineData("john.doenew@example.com")]
    [InlineData("john.doenew@example.com", 2)]
    [InlineData("jane.smith@example.com")]
    [InlineData("jane.smith@example.com", 2)]
    [InlineData("john.doe@example.com")]
    [InlineData("john.doe@example.com", 2)]
    public async Task CreateEmployeeByEmployerAsync_ReturnsNewEmployeeId(string email, int creatorId = 1)
    {
        // Arrange
        var expected = new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            PhoneNumber = "1234567890",
            Department = "HR"
        };

        using var connection = context.CreateConnection();

        // Act
        expected.Id = await repository.CreateOrAppendByEmployerAsync(expected, creatorId);

        var actual = await connection.QuerySingleAsync<Employee>(
            "SELECT * FROM Employees WHERE Id = @Id", new { Id = expected.Id });
        var employeeEmployeesRow = new { EmployeeId = expected.Id, EmployerId = creatorId, };

        dynamic? res = await connection.QuerySingleOrDefaultAsync(
            "select * from EmployeeEmployers where employeeId = @Id1 and employerId = @Id2",
            new { Id1 = employeeEmployeesRow.EmployeeId, Id2 = employeeEmployeesRow.EmployerId });

        // Assert
        //AssertHelper.AssertEmployeesAreEqual(expected, actual);
        Assert.NotNull(res);
        Assert.Equal(employeeEmployeesRow.EmployerId, res!.EmployerId);
        Assert.Equal(employeeEmployeesRow.EmployeeId, res!.EmployeeId);
        Assert.True(res.Id > 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task UpdateEmployeeAsync_ReturnsNumberOfAffectedRows(int id)
    {
        // Arrange
        var employee = new Employee
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Department = "HR"
        };
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.UpdateEmployeeAsync(employee);
        var actualEmployee = await connection.QuerySingleAsync<Employee>(
            "SELECT * FROM Employees WHERE Id = @Id", new { employee.Id });

        employee.Email = actualEmployee.Email;

        // Assert
        Assert.Equal(1, result);
        AssertHelper.AssertEmployeesAreEqual(employee, actualEmployee);
    }

    [Theory]
    [InlineData(1)]
    public async Task DeleteEmployeeAsync_ReturnsNumberOfAffectedRows(int id)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.DeleteEmployeeAsync(id);

        var deletedEmployee = await connection.QuerySingleOrDefaultAsync<Employee>(
            "SELECT * FROM Employees WHERE Id = @Id", new { id });

        // Assert
        Assert.True(deletedEmployee!.IsDeleted);
    }

    [Theory]
    [InlineData("john.doe@example.com", false, 1)]
    [InlineData("jane.smith@example.com", false, 2)]
    [InlineData("john.deleted@example.com", true)]
    public async Task GetEmployeeByEmailAsync_ReturnsEmployee(string email, bool isNull, int expectedId = 0)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.GetEmployeeByEmailAsync(email);

        // Assert
        if (isNull)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
    }

    public void Dispose()
    {
        fixture.Dispose();
    }
}

