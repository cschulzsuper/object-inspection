using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Authentication;
using Super.Paula.Client.Storage;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Client.Security
{
    public class BadgeStorage
    {
        private readonly ILocalStorage _localStorage;
        private readonly IServiceProvider _serviceProvider;
        
        private static Func<string?, Task>? _onChangeAction;

        private string? _badge;
        private bool _verified;
        private bool _verifying;

        public BadgeStorage(
            ILocalStorage localStorage,
            IServiceProvider serviceProvider)
        {
            _localStorage = localStorage;
            _serviceProvider = serviceProvider;
        }

        private IAuthenticationRequestHandler AuthenticationRequestHandler => _serviceProvider.GetRequiredService<IAuthenticationRequestHandler>();

        public void OnChange(Func<string?, Task> action)
        {
            _onChangeAction = action;
        }

        public async Task SetAsync(string? badge)
        {
            if (badge == null)
            {
                await _localStorage.RemoveItemAsync("badge");
                _badge = null;
                await (_onChangeAction?.Invoke(null) ?? Task.CompletedTask);
            }
            else
            {
                await _localStorage.SetItemAsync("badge", badge);
                _badge = badge;
                await (_onChangeAction?.Invoke(badge) ?? Task.CompletedTask);
            }
        }

        public async Task<string?> GetOrDefaultAsync()
        {
            _badge ??= await GetOrDefaultLocalAsync();

            if (_verified || _verifying)
            {
                return _badge;
            }

            try
            {
                _verifying = true;
                await AuthenticationRequestHandler.VerifyAsync();
            }
            finally
            {
                _verifying = false;
            }

            _verified = true;

            return _badge;
        }

        public async Task<string?> GetOrDefaultLocalAsync()
            => await _localStorage.GetItemAsync<string>("badge");
    }
}
