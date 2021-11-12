using Blazored.SessionStorage;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client
{
    public class DefaultSessionStorage : ISessionStorage
    {
        private readonly ISessionStorageService _sessionStorageService;

        public DefaultSessionStorage(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
            => _sessionStorageService.ContainKeyAsync(key);

        public async ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            var result = await _sessionStorageService.GetItemAsStringAsync(key);
            var deserializedResult = JsonSerializer.Deserialize<T>(result);
            return deserializedResult!;
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
            => _sessionStorageService.RemoveItemAsync(key);

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            var serializedData = JsonSerializer.Serialize(data);
            await _sessionStorageService.SetItemAsStringAsync(key, serializedData);
        }
    }
}
