﻿using Dapper;
using DataAccess;

namespace Tests.DataAccess;

public class SqlServerDatabaseFixture : IDisposable
{
    public DataContext DataContext { get; private set; }
    private readonly string connectionString = "Data Source=ps00024;Database=EMSdb_Test;Integrated Security=True;";

    public SqlServerDatabaseFixture()
    {
        DataContext = new DataContext(connectionString);
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = DataContext.CreateConnection();
        connection.Open();

        string createTableSql = GetSql("C:\\Users\\garik.mikayelyan\\source\\repos"
            + "\\EMS\\Tests\\DataAccess\\SQLs\\CreateTables.sql");

        connection.Execute(createTableSql);
    }

    public void Dispose()
    {
        using var connection = DataContext.CreateConnection();
        connection.Open();

        string cleanupSql = GetSql("C:\\Users\\garik.mikayelyan\\source\\" +
            "repos\\EMS\\Tests\\DataAccess\\SQLs\\DropTables.sql");

        connection.Execute(cleanupSql);
    }

    private string GetSql(string path)
    {
        return File.ReadAllText(path);
    }
}

[CollectionDefinition(nameof(SqlServerCollection))]
public class SqlServerCollection : ICollectionFixture<SqlServerDatabaseFixture>
{
}

