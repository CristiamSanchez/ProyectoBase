using System.Net.Http.Headers;
using System.Net.Http.Json;
using Proyecto.Application.DTOs.Auth;

namespace Proyecto.IntegrationTests.Infrastructure;

public static class AuthHelper
{
    public static async Task<string> LoginAsync(
        HttpClient client,
        string username,
        string password)
    {
        var request = new LoginRequestDto
        {
            Username = username,
            Password = password
        };

        var response = await client.PostAsJsonAsync(
            "/api/v1/Auth/login",
            request);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        return result!.Token;
    }

    public static async Task AuthenticateAsync(
        HttpClient client,
        string username,
        string password)
    {
        var token =
            await LoginAsync(client, username, password);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);
    }
}