using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory;

public interface IBusinessObjectRequestHandler
{
    ValueTask<BusinessObjectResponse> GetAsync(string businessObject);
    IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default);
    ValueTask<SearchBusinessObjectResponse> SearchAsync(string query);

    ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request);
    ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request);
    ValueTask DeleteAsync(string businessObject, string etag);
}