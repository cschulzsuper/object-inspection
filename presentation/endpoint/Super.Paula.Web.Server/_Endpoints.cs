using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Administration;
using Super.Paula.Auditing;
using Super.Paula.Guidlines;
using Super.Paula.Inventory;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapPaulaServer(this IEndpointRouteBuilder endpoints)
            => endpoints
                .MapAccount()
                .MapBusinessObject()
                .MapBusinessObjectInspectionAudit()
                .MapInspection()
                .MapInspector()
                .MapOrganization();
    }
}