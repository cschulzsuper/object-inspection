using System.Threading.Tasks;
using Super.Paula.Application.Guidelines.Requests;
using Super.Paula.Application.Guidelines.Responses;

namespace Super.Paula.Application.Guidelines
{
    public interface IInspectionHandler : IInspectionProvider
    {
        ValueTask<InspectionResponse> CreateAsync(InspectionRequest request);
        ValueTask ReplaceAsync(string inspection, InspectionRequest request);
        ValueTask DeleteAsync(string inspection);

        ValueTask ActivateAsync(string inspection);
        ValueTask DeactivateAsync(string inspection);

    }
}