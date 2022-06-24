using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Client.Local
{
    public interface ILocalStorage
    {
        event EventHandler<LocalStorageEventArgs>? Changed;

        ValueTask<T?> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null);

        ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null);
    }
}
