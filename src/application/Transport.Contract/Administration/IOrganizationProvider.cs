using Super.Paula.Application.Administration.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IOrganizationProvider
    {
        ValueTask<OrganizationResponse> GetAsync(string organization);
        IAsyncEnumerable<OrganizationResponse> GetAll();
    }
}