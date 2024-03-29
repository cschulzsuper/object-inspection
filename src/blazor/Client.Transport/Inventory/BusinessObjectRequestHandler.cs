﻿using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Application.Inventory.Requests;
using ChristianSchulz.ObjectInspection.Application.Inventory.Responses;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.ErrorHandling;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Client.Inventory;

public class BusinessObjectRequestHandler : IBusinessObjectRequestHandler
{
    private readonly HttpClient _httpClient;

    public BusinessObjectRequestHandler(
        HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(appSettings.Server);
    }

    public async ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("business-objects", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectResponse>())!;
    }

    public async ValueTask DeleteAsync(string businessObject, string etag)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"business-objects/{businessObject}");
        request.Headers.TryAddWithoutValidation("If-Match", etag);

        var responseMessage = await _httpClient.SendAsync(request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects?q={query}&s={skip}&t={take}", cancellationToken);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
        var response = JsonSerializer.DeserializeAsyncEnumerable<BusinessObjectResponse>(
            responseStream, CustomJsonSerializerOptions.WebResponse, cancellationToken);

        await foreach (var responseItem in response
            .WithCancellation(cancellationToken))
        {
            yield return responseItem!;
        }
    }

    public async ValueTask<BusinessObjectResponse> GetAsync(string businessObject)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects/{businessObject}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<BusinessObjectResponse>())!;
    }

    public async ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"business-objects/{businessObject}", request);

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();
    }

    public async ValueTask<SearchBusinessObjectResponse> SearchAsync(string query)
    {
        var responseMessage = await _httpClient.GetAsync($"business-objects/search?q={query}");

        responseMessage.RuleOutProblems();
        responseMessage.EnsureSuccessStatusCode();

        return (await responseMessage.Content.ReadFromJsonAsync<SearchBusinessObjectResponse>())!;
    }
}