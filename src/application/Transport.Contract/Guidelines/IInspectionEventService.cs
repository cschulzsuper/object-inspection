using System.Threading.Tasks;

namespace Super.Paula.Application.Guidelines
{
    public interface IInspectionEventService
    {
        ValueTask CreateInspectionEventAsync(Inspection inspection);
        ValueTask CreateInspectionDeletionEventAsync(string inspection);
    }
}