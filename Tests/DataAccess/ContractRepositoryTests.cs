using DataAccess.Repositories;
using DataAccess;
using DataAccess.Enities;
using Dapper;
using DataAccess.Interfaces;

namespace Tests.DataAccess;

public class ContractRepositoryTests : IDisposable
{
    private readonly SqlServerDatabaseFixture fixture;
    private readonly DataContext context;
    private readonly IContractRepository repository;

    public ContractRepositoryTests()
    {
        fixture = new SqlServerDatabaseFixture();
        context = fixture.DataContext;
        repository = new ContractRepository(context);
    }

    [Theory]
    [InlineData(3)]
    public async Task GetAllContractsAsync_ReturnsAllContracts(int expectedCount)
    {
        // Arrange

        // Act
        var result = await repository.GetAllContractsAsync();
        int count = result.Count();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(1, 2, 1, 2)]
    [InlineData(2, 1, 3)]
    public async Task GetContractsByEmployeeEmployersIdAsync_ReturnsContracts(int employerId, int count,
        params int[] employeeEmployersIdArr)
    {
        // Arrange               

        // Act
        var result = await repository.GetContractsByEmployerIdAsync(employerId);
        var arr = result.ToArray();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, arr.Length);

        for (int i = 0; i < arr.Length; i++)
        {
            Assert.Equal(employeeEmployersIdArr[i], arr[i].EmployeeEmployersId);
        }       
    }

    [Fact]
    public async Task GetContractByIdAsync_ReturnsContract()
    {
        // Arrange
        var id = 2;

        // Act
        var result = await repository.GetContractByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CreateContractAsync_ReturnsNewContractId()
    {
        // Arrange
        var expected = new Contract
        {
            ContractType = "FullTime",
            Description = "Full-time contract",
            Salary = 50000,
            StartDate = DateTime.UtcNow,
            EmployeeEmployersId = 1,
        };
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.CreateContractAsync(expected);

        expected.Id = result;
        var actual = await connection.QuerySingleAsync<Contract>("Select * from Contracts where id = @id", new { Id = result });

        // Assert
        Assert.True(result > 0);
        AssertHelper.AssertContractsAreEqual(expected, actual);
    }

    [Fact]
    public async Task UpdateContractAsync_ReturnsNumberOfAffectedRows()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            ContractType = "FullTime",
            Description = "Full-time contract",
            Salary = 50000,
            StartDate = DateTime.UtcNow,
            EmployeeEmployersId = 1,
        };
        using var connection = context.CreateConnection();

        // Act
        var result = await repository.UpdateContractAsync(contract);
        var actualContract = await connection.QuerySingleAsync<Contract>("Select * from Contracts where id = @id", new { Id = contract.Id });

        // Assert
        Assert.Equal(1, result);
        AssertHelper.AssertContractsAreEqual(contract, actualContract);
    }

    [Fact]
    public async Task DeleteContractAsync_ReturnsNumberOfAffectedRows()
    {
        // Arrange
        using var connection = context.CreateConnection();
        Contract? contract;
        var id = 1;

        // Act
        var result = await repository.DeleteContractAsync(id);

        contract = await connection.QuerySingleOrDefaultAsync<Contract>("select * from Contracts where Id = @Id", new { Id = id });

        // Assert
        Assert.Null(contract);
    }

    public void Dispose()
    {
        fixture.Dispose();
    }
}

