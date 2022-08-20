using Microsoft.AspNetCore.Identity;
using Super.Paula.Application.Auth.Requests;
using Super.Paula.Application.Auth.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auth;

public class IdentityRequestHandler : IIdentityRequestHandler
{
    private readonly IIdentityManager _identityManager;
    private readonly IPasswordHasher<Identity> _passwordHasher;

    public IdentityRequestHandler(
        IIdentityManager identityManager,
        IPasswordHasher<Identity> passwordHasher)
    {
        _identityManager = identityManager;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<IdentityResponse> CreateAsync(IdentityRequest request)
    {
        var entity = new Identity
        {
            UniqueName = request.UniqueName,
            MailAddress = request.MailAddress
        };

        entity.Secret = _passwordHasher.HashPassword(entity, "default");

        await _identityManager.InsertAsync(entity);

        return new IdentityResponse
        {
            ETag = entity.ETag,
            UniqueName = entity.UniqueName,
            MailAddress = entity.MailAddress
        };
    }

    public async ValueTask DeleteAsync(string identity, string etag)
    {
        var entity = await _identityManager.GetAsync(identity);

        entity.ETag = etag;

        await _identityManager.DeleteAsync(entity);
    }

    public IAsyncEnumerable<IdentityResponse> GetAll()
        => _identityManager
            .GetAsyncEnumerable(query => query
            .Select(entity => new IdentityResponse
            {
                ETag = entity.ETag,
                UniqueName = entity.UniqueName,
                MailAddress = entity.MailAddress
            }));

    public async ValueTask<IdentityResponse> GetAsync(string identity)
    {
        var entity = await _identityManager.GetAsync(identity);

        return new IdentityResponse
        {
            ETag = entity.ETag,
            UniqueName = entity.UniqueName,
            MailAddress = entity.MailAddress
        };
    }

    public async ValueTask ReplaceAsync(string identity, IdentityRequest request)
    {
        var entity = await _identityManager.GetAsync(identity);

        entity.UniqueName = request.UniqueName;
        entity.MailAddress = request.MailAddress;
        entity.ETag = request.ETag;

        await _identityManager.UpdateAsync(entity);
    }
}