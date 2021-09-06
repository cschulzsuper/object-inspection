using Super.Paula.Web.Shared.Handling.Responses;
using System.Threading.Tasks;
using Super.Paula.Web.Shared.Handling.Requests;

namespace Super.Paula.Web.Shared.Handling
{
    public interface IInspectionBusinessObjectHandler
    {
        ValueTask<InspectionBusinessObjectResponse> CreateAsync(InspectionBusinessObjectRequest request);
        ValueTask DeleteAsync(string inspection, string businessObject);
        ValueTask RefreshInspectionAsync(string inspection, RefreshInspectionRequest request);
    }
}