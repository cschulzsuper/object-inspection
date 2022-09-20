using ChristianSchulz.ObjectInspection.RuntimeData;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class ConnectionViolation : IRuntimeData
{
    public string Correlation => Violator;

    public string Violator { get; set; } = string.Empty;

    public int ViolationCounter { get; set; } = 0;
}