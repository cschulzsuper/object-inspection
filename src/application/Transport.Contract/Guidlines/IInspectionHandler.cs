using Super.Paula.Application.Guidlines.Requests;
using Super.Paula.Application.Guidlines.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Guidlines
{
    public interface IInspectionHandler
    {
        ValueTask<InspectionResponse> GetAsync(string inspection);
        IAsyncEnumerable<InspectionResponse> GetAll();

        ValueTask<InspectionResponse> CreateAsync(InspectionRequest request);
        ValueTask ReplaceAsync(string inspection, InspectionRequest request);
        ValueTask DeleteAsync(string inspection);

        ValueTask ActivateAsync(string inspection);
        ValueTask DeactivateAsync(string inspection);

    }
}