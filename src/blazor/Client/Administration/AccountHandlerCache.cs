using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AccountHandlerCache
    {
        public Task<QueryAuthorizationsResponse?>? QueryAuthorizationsResponse { get; set; } = null;

        public string? FallbackOrganization { get; set; } = null;
        public string? FallbackInspector { get; set; } = null;
        public string? FallbackBearer { get; set; } = null;
    }
}
