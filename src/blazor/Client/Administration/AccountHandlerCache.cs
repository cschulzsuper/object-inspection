using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Client.Administration
{
    public class AccountHandlerCache
    {
        public Task<QueryAuthorizationsResponse?>? QueryAuthorizationsResponse { get; set; } = null;
    }
}
