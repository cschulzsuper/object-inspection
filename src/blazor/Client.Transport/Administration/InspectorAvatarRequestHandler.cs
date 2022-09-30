using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Administration;

public class InspectorAvatarRequestHandler : IInspectorAvatarRequestHandler
{
    private readonly HttpClient _httpClient;

    public InspectorAvatarRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask DeleteAsync(string inspector)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"inspectors/{inspector}/avatar");

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask<Stream> ReadAsync(string inspector)
    {
        var responseMessage = await _httpClient.GetAsync($"inspectors/{inspector}/avatar");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return await responseMessage.Content.ReadAsStreamAsync();
    }

    public async ValueTask WriteAsync(Stream stream, string inspector)
    {
        using var streamContent = new StreamContent(stream);

        var responseMessage = await _httpClient.PutAsync($"inspectors/{inspector}/avatar", streamContent);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }
}