using Super.Paula.Application.Administration.Responses;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorStreamer
    {
        Task StreamInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response);

        Task StreamInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response);

        Task StreamInspectorBusinessObjectDeletionAsync(string inspector, string businessObject);
    }
}
