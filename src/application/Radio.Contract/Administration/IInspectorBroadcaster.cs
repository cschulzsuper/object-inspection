using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IInspectorBroadcaster
{
    Task SendInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response);

    Task SendInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response);

    Task SendInspectorBusinessObjectDeletionAsync(string inspector, string businessObject);
}