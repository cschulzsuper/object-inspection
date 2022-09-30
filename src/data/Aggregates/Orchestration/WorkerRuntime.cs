using ChristianSchulz.ObjectInspection.RuntimeData;

namespace ChristianSchulz.ObjectInspection.Application.Orchestration;

public class WorkerRuntime : IRuntimeData
{
    public string Correlation => Worker;

    public string Worker { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public int HeartbeatDate { get; set; }

    public int HeartbeatTime { get; set; }
}