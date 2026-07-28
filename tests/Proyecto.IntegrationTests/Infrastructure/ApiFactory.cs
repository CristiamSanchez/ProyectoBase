using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Proyecto.IntegrationTests.Infrastructure;

public class ApiFactory : WebApplicationFactory<Program>
{
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