using ChristianSchulz.ObjectInspection.Application.Guidelines.Requests;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines;

public interface IInspectionRequestHandler
{
    ValueTask<InspectionResponse> GetAsync(string inspection);
    IAsyncEnumerable<InspectionResponse> GetAll();

    ValueTask<InspectionResponse> CreateAsync(InspectionRequest request);
    ValueTask ReplaceAsync(string inspection, InspectionRequest request);
    ValueTask DeleteAsync(string inspection, string etag);

    ValueTask<ActivateInspectionResponse> ActivateAsync(string inspection, string etag);
    ValueTask<DeactivateInspectionResponse> DeactivateAsync(string inspection, string etag);

}