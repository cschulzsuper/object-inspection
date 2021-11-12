using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client
{
    public class ProtectedSessionStorage : ISessionStorage
    {
        private readonly Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage _protectedSessionStorage;

        public ProtectedSessionStorage(Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage protectedSessionStorage)
        {
            _protectedSessionStorage = protectedSessionStorage;
        }

        public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
        {
            var result = await _protectedSessionStorage.GetAsync<string>("secret", key);
            return result.Success;   
        }

        public async ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            var result = await _protectedSessionStorage.GetAsync<string>("secret", key);
            var deserializedResult = JsonSerializer.Deserialize<T>(result.Value!);
            return deserializedResult!;
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
            => _protectedSessionStorage.DeleteAsync(key);

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            var serializedData = JsonSerializer.Serialize(data);
            await _protectedSessionStorage.SetAsync("secret", key, serializedData);
        }
    }
}
