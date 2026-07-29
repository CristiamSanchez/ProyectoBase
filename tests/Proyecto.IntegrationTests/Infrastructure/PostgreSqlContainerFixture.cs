using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Proyecto.Infrastructure.Data;

namespace Proyecto.IntegrationTests.Infrastructure;

public class PostgreSqlContainerFixture : IAsyncLifetime
{
    private readonly IContainer _container;

    public string ConnectionString { get; private set; } = string.Empty;

    public PostgreSqlContainerFixture()
    {
        _container = new ContainerBuilder("postgres:14-alpine")
            .WithEnvironment("POSTGRES_DB", "proyecto_test")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilMessageIsLogged(
                        "database system is ready to accept connections"))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var port = _container.GetMappedPublicPort(5432);

        ConnectionString =
            $"Host=localhost;" +
            $"Port={port};" +
            $"Database=proyecto_test;" +
            $"Username=postgres;" +
            $"Password=postgres;" +
            $"Pooling=false;";

        await WaitForDatabaseAsync();
    }

    private async Task WaitForDatabaseAsync()
    {
        const int maxAttempts = 30;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var connection =
                    new NpgsqlConnection(ConnectionString);

                await connection.OpenAsync();

                await connection.CloseAsync();

                return;
            }
            catch (NpgsqlException) when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        throw new InvalidOperationException(
            "PostgreSQL no estuvo disponible después de 30 intentos.");
    }

    public async Task ApplyMigrationsAsync()
    {
        await using var context = CreateDbContext();

        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();

        await _container.DisposeAsync();
    }

    public AppDbContext CreateDbContext()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        return new AppDbContext(options);
    }
}