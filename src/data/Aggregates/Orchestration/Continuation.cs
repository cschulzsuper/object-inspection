namespace ChristianSchulz.ObjectInspection.Application.Orchestration;

public class Continuation
{
    public long Id { get; set; }

    public string ETag { get; set; } = string.Empty;

    public string ContinuationId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string OperationId { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public int CreationDate { get; set; }

    public int CreationTime { get; set; }
}