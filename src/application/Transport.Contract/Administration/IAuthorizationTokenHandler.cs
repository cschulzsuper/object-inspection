using Super.Paula.Shared.Security;

namespace Super.Paula.Application.Administration;

public interface IAuthorizationTokenHandler
{
    public void RewriteAuthorizations(Token token);
}