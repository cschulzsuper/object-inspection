using ChristianSchulz.ObjectInspection.Application.Inventory.Requests;
using ChristianSchulz.ObjectInspection.Application.Inventory.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public interface IBusinessObjectRequestHandler
{
    ValueTask<BusinessObjectResponse> GetAsync(string businessObject);
    IAsyncEnumerable<BusinessObjectResponse> GetAll(string query, int skip, int take, CancellationToken cancellationToken = default);
    ValueTask<SearchBusinessObjectResponse> SearchAsync(string query);

    ValueTask<BusinessObjectResponse> CreateAsync(BusinessObjectRequest request);
    ValueTask ReplaceAsync(string businessObject, BusinessObjectRequest request);
    ValueTask DeleteAsync(string businessObject, string etag);
}