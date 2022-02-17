using Super.Paula.Authorization;

namespace Super.Paula.Application.Administration
{
    public interface ITokenAuthorizationFilter
    {
        public void Apply(Token token);
    }
}
