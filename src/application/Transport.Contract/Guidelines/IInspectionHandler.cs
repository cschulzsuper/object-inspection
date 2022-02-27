using Super.Paula.Application.Guidelines.Requests;
using Super.Paula.Application.Guidelines.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Guidelines
{
    public interface IInspectionHandler
    {
        ValueTask<InspectionResponse> GetAsync(string inspection);
        IAsyncEnumerable<InspectionResponse> GetAll();

        ValueTask<InspectionResponse> CreateAsync(InspectionRequest request);
        ValueTask ReplaceAsync(string inspection, InspectionRequest request);
        ValueTask DeleteAsync(string inspection, string etag);

        ValueTask<ActivateInspectionResponse> ActivateAsync(string inspection, string etag);
        ValueTask<DeactivateInspectionResponse> DeactivateAsync(string inspection, string etag);

    }
}