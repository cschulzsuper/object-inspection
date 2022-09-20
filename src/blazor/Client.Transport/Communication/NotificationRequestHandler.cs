using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Communication.Requests;
using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Communication;

public sealed class NotificationRequestHandler : INotificationRequestHandler
{
    private readonly HttpClient _httpClient;

    public NotificationRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
    }

    public async ValueTask DeleteAsync(string inspector, int date, int time, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"inspectors/{inspector}/notifications/{date}/{time}");
        request.Headers.Add("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<NotificationResponse> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("notifications");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<NotificationResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async IAsyncEnumerable<NotificationResponse> GetAllForInspector(string inspector)
    {
        var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/notifications");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        var response = JsonSerializer.DeserializeAsyncEnumerable<NotificationResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse);

        await foreach (var responseItem in response)
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time)
    {
        var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/notifications/{date}/{time}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<NotificationResponse>())!;
    }

    public async ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"inspectors/{inspector}/notifications/{date}/{time}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}