using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Client.Handling
{
    public class AccountHandlerCache
    {
        public Task<QueryAuthorizationsResponse?>? QueryAuthorizationsResponse { get; set; } = null;

        public string? FallbackOrganization { get; set; } = null;
        public string? FallbackInspector { get; set; } = null;
        public string? FallbackBearer { get; set; } = null;
    }
}
