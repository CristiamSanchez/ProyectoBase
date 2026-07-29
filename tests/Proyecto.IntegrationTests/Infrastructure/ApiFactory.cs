using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Proyecto.Infrastructure.Data;

namespace Proyecto.IntegrationTests.Infrastructure;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainerFixture _postgresFixture;

    public ApiFactory()
    {
        _postgresFixture = new PostgreSqlContainerFixture();
    }

    public async Task InitializeAsync()
    {
        await _postgresFixture.InitializeAsync();

        await _postgresFixture.ApplyMigrationsAsync();

        await using var context =
            _postgresFixture.CreateDbContext();

        await TestDataSeeder.SeedAsync(context);
    }

    public async Task DisposeAsync()
    {
        await _postgresFixture.DisposeAsync();
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(
            (context, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] =
                            _postgresFixture.ConnectionString,

                        ["Jwt:Key"] =
                            "ClaveSuperSecretaParaPruebasIntegrationTests123456789",

                        ["Jwt:Issuer"] =
                            "ProyectoApi",

                        ["Jwt:Audience"] =
                            "ProyectoApiUsers"
                    });
            });
    }
}