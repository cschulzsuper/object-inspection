using Super.Paula.Web.Shared.Handling.Requests;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Shared.Handling
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