using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft = Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChristianSchulz.ObjectInspection.Client.Storage;

public class ProtectedLocalStorage : ILocalStorage
{
    private readonly Microsoft::ProtectedLocalStorage _protectedLocalStorage;

    public ProtectedLocalStorage(Microsoft::ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public event EventHandler<LocalStorageEventArgs>? Changed;

    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
    {
        var result = await _protectedLocalStorage.GetAsync<string>("secret", key);
        return result.Success;
    }

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
    {
        var result = await _protectedLocalStorage.GetAsync<string>("secret", key);

        var deserializedResult = result.Value != null
            ? JsonSerializer.Deserialize<T>(result.Value)
            : default;

        return deserializedResult;
    }

    public async ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
    {
        await _protectedLocalStorage.DeleteAsync(key);

        Changed?.Invoke(this, new LocalStorageEventArgs(key));
    }

    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
    {
        var serializedData = JsonSerializer.Serialize(data);
        await _protectedLocalStorage.SetAsync("secret", key, serializedData);

        Changed?.Invoke(this, new LocalStorageEventArgs(key));
    }
}