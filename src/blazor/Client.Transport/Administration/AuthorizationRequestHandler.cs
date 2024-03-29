﻿using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Administration;

public class AuthorizationRequestHandler : IAuthorizationRequestHandler
{
    private readonly HttpClient _httpClient;

    public AuthorizationRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<string> AuthorizeAsync(string organization, string inspector)
    {
        var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/inspectors/{inspector}/authorize", null);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadAsStringAsync())!;
    }

    public async ValueTask<string> StartImpersonationAsync(string organization, string inspector)
    {
        var responseMessage = await _httpClient.PostAsync($"organizations/{organization}/inspectors/{inspector}/impersonate", null);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadAsStringAsync())!;
    }

    public async ValueTask<string> StopImpersonationAsync()
    {
        var responseMessage = await _httpClient.PostAsync("inspectors/me/unmask", null);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadAsStringAsync())!;
    }
}