using Super.Paula.Authorization;

namespace Super.Paula.Application.Administration
{
    public interface IAuthorizationTokenHandler
    {
        public void RewriteAuthorizations(Token token);
    }
}
