using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    internal class IdentityHandler : IIdentityHandler
    {
        private readonly IIdentityManager _identityManager;

        public IdentityHandler(IIdentityManager identityManager) 
        {
            _identityManager = identityManager;
        }

        public async ValueTask<IdentityResponse> CreateAsync(IdentityRequest request)
        {
            var entity = new Identity
            {
                UniqueName = request.UniqueName,
                MailAddress = request.MailAddress,
                Secret = "default"
            };

            await _identityManager.InsertAsync(entity);

            return new IdentityResponse
            {
                UniqueName = entity.UniqueName,
                MailAddress = entity.MailAddress
            };
        }

        public async ValueTask DeleteAsync(string identity)
        {
            var entity = await _identityManager.GetAsync(identity);

            await _identityManager.DeleteAsync(entity);
        }

        public IAsyncEnumerable<IdentityResponse> GetAll()
            => _identityManager
                .GetAsyncEnumerable(query => query
                .Select(entity => new IdentityResponse
                {
                    UniqueName = entity.UniqueName,
                    MailAddress = entity.MailAddress
                }));

        public async ValueTask<IdentityResponse> GetAsync(string identity)
        {
            var entity = await _identityManager.GetAsync(identity);
         
            return new IdentityResponse
            {
                UniqueName = entity.UniqueName,
                MailAddress = entity.MailAddress
            };
        }

        public async ValueTask ReplaceAsync(string identity, IdentityRequest request)
        {
            var entity = await _identityManager.GetAsync(identity);

            entity.UniqueName = request.UniqueName;
            entity.MailAddress = request.MailAddress;

            await _identityManager.UpdateAsync(entity);
        }

        public async ValueTask ResetAsync(string identity)
        {
            var entity = await _identityManager.GetAsync(identity);

            entity.Secret = "default";

            await _identityManager.UpdateAsync(entity);
        }
    }
}