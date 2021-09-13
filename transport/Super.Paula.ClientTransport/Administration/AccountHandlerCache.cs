using Super.Paula.Administration.Responses;

namespace Super.Paula.Administration
{
    internal class AccountHandlerCache
    {
        public Task<QueryAuthorizationsResponse?>? QueryAuthorizationsResponse { get; set; } = null;

        public string? FallbackOrganization { get; set; } = null;
        public string? FallbackInspector { get; set; } = null;
        public string? FallbackBearer { get; set; } = null;
    }
}
