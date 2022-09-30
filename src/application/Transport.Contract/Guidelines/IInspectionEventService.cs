using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines;

public interface IInspectionEventService
{
    ValueTask CreateInspectionUpdateEventAsync(Inspection inspection);
    ValueTask CreateInspectionDeletionEventAsync(string inspection);
}