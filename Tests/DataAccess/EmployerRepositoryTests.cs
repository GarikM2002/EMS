using DataAccess.Enities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAccess;
using Dapper;
using Services.Auth;

namespace Tests.DataAccess;

public class EmployerRepositoryTests : IDisposable
{
    private readonly SqlServerDatabaseFixture fixture;
    private readonly DataContext context;
    private readonly IEmployerRepository repository;

    public EmployerRepositoryTests()
    {
        fixture = new SqlServerDatabaseFixture();
        context = fixture.DataContext;
        repository = new EmployerRepository(context);
    }

    [Theory]
    [InlineData(2)]
    public async Task GetAllEmployersAsync_ReturnsAllEmployers(int expectedCount)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = (await repository.GetAllEmployersAsync()).ToArray();

        Employer[] expected = (await connection.QueryAsync<Employer>
            ("SELECT * FROM Employers Where isdeleted = 0")).ToArray();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Length);

        for (int i = 0; i < expected.Length; i++)
        {
            AssertHelper.AssertEmployersAreEqual(expected[i], result[i]);
        }
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    public async Task GetEmployerByIdAsync_ReturnsEmployer(int id, bool isNull)
    {
        // Arrange       
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.GetEmployerByIdAsync(id);
        Employer? employer = await connection.QuerySingleOrDefaultAsync<Employer?>(
            "Select * from employers where Id = @Id AND isdeleted = 0", new { Id = id });

        // Assert
        if (isNull)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        AssertHelper.AssertEmployersAreEqual(employer!, result);

    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3, true)]
    public async Task UpdateEmployerAsync_ReturnsNumberOfAffectedRows(int id, bool isDeleted = false)
    {
        // Arrange
        var (hash, salt) = AuthHelper.CreatePasswordHash("123456");
        var expected = new Employer
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Department = "HR",
            PasswordHash = hash,
            PasswordSalt = salt,
        };

        using var connection = context.CreateConnection();

        // Act
        var result = await repository.UpdateEmployerAsync(expected);
        var actualEmployer = await connection.QuerySingleAsync<Employer>(
            "SELECT * FROM Employers WHERE Id = @Id", new { expected.Id });

        expected.Email = actualEmployer.Email;

        // Assert
        Assert.Equal(1, result);
        AssertHelper.AssertEmployersAreEqual(expected, actualEmployer, !isDeleted);
    }

    [Theory]
    [InlineData(1)]
    public async Task DeleteEmployerAsync_ReturnsNumberOfAffectedRows(int id)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.DeleteEmployerAsync(id);

        var deletedEmployer = await connection.QuerySingleOrDefaultAsync<Employer>(
            "SELECT * FROM Employers WHERE Id = @Id", new { id });

        // Assert
        Assert.True(deletedEmployer!.IsDeleted);
    }

    [Theory]
    [InlineData("alice.johnson@example.com", false, 1)]
    [InlineData("bob.brown@example.com", false, 2)]
    [InlineData("alice.johnsonDeleted@example.com", true)]
    public async Task GetEmployerByEmailAsync_ReturnsEmployee(string email, bool isNull, int expectedId = 0)
    {
        // Arrange
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.GetEmployerByEmailAsync(email);

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
