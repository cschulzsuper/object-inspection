using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Application.Authentication.Exceptions;
using ChristianSchulz.ObjectInspection.Application.Authentication.Requests;
using ChristianSchulz.ObjectInspection.Application.Authentication.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;

namespace ChristianSchulz.ObjectInspection.Client.Authentication;

public class AuthenticationRequestHandler : IAuthenticationRequestHandler
{
    private readonly HttpClient _httpClient;

    public AuthenticationRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask VerifyAsync()
    {
        var responseMessage = await _httpClient.PostAsync("identities/me/verify", null);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask ChangeSecretAsync(ChangeIdentitySecretRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("identities/me/change-secret", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask RegisterAsync(RegisterIdentityRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("identities/register", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask<string> SignInAsync(string identity, SignInIdentityRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"identities/{identity}/sign-in", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadAsStringAsync())!;
    }

    public async ValueTask SignOutAsync()
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("identities/me/sign-out", null);

            responseMessage.RuleOutProblems();
            responseMessage.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException exception)
        {
            throw new SignOutException($"Could not sign out.", exception);
        }
    }

    public async ValueTask<ResetIdentityResponse> ResetAsync(string identity, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"identities/{identity}/reset");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<ResetIdentityResponse>())!;
    }
}