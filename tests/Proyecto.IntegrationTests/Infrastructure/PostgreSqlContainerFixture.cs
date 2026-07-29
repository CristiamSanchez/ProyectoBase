
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
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
