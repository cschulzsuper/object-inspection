using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using msft = Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Super.Paula.Client.Storage
{
    public class ProtectedLocalStorage : ILocalStorage
    {
        private readonly msft::ProtectedLocalStorage _protectedLocalStorage;

        public ProtectedLocalStorage(msft::ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

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

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
            => _protectedLocalStorage.DeleteAsync(key);

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            var serializedData = JsonSerializer.Serialize(data);
            await _protectedLocalStorage.SetAsync("secret", key, serializedData);
        }
    }
}
