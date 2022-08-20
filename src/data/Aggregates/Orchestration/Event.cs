namespace Super.Paula.Application.Orchestration;

public class Event
{
    public string ETag { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string OperationId { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public int CreationDate { get; set; }

    public int CreationTime { get; set; }
}