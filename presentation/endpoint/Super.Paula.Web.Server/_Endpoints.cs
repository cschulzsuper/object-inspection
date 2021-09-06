using Microsoft.AspNetCore.Routing;
using Super.Paula.Web.Server.Endpoints;

namespace Super.Paula.Web.Server
{
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapPaulaWebServer(this IEndpointRouteBuilder endpoints)
            => endpoints
                .MapAccount()
                .MapBusinessObject()
                .MapBusinessObjectInspectionAudit()
                .MapInspection()
                .MapInspector()
                .MapOrganization();
    }
}