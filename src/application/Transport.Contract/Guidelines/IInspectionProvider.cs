using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Guidelines.Responses;

namespace Super.Paula.Application.Guidelines
{
    public interface IInspectionProvider
    {
        ValueTask<InspectionResponse> GetAsync(string inspection);
        IAsyncEnumerable<InspectionResponse> GetAll();
    }
}