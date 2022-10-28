using ChristianSchulz.ObjectInspection.Application.Operation.Requests;
using ChristianSchulz.ObjectInspection.Application.Operation.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IDistinctionTypeRequestHandler
{
    ValueTask<DistinctionTypeResponse> CreateAsync(DistinctionTypeRequest request);
    ValueTask DeleteAsync(string uniqueName, string etag);
    IAsyncEnumerable<DistinctionTypeResponse> GetAll();
    ValueTask<DistinctionTypeResponse> GetAsync(string uniqueName);

    ValueTask<DistinctionTypeFieldCreateResponse> CreateFieldAsync(string uniqueName, DistinctionTypeFieldRequest request);
    ValueTask<DistinctionTypeFieldDeleteResponse> DeleteFieldAsync(string uniqueName, string field, string etag);
}