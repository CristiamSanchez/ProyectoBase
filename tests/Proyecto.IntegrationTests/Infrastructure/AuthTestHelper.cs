using System.Net.Http.Headers;
using System.Net.Http.Json;
using Proyecto.Application.DTOs.Auth;

namespace Proyecto.IntegrationTests.Infrastructure;

public static class AuthTestHelper
{
    public static async Task<HttpClient> CreateAuthenticatedClientAsync(
        ApiFactory factory,
        string username,
        string password)
    {
        var client = factory.CreateClient();

        var loginRequest = new LoginRequestDto
        {
            Username = username,
            Password = password
        };

        var response = await client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<LoginResponseDto>();

        if (result == null || string.IsNullOrWhiteSpace(result.Token))
        {
            throw new InvalidOperationException(
                "No fue posible obtener el token JWT.");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                result.Token);

        return client;
    }
}