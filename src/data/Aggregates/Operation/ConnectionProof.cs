namespace ChristianSchulz.ObjectInspection.Application.Operation;

public record ConnectionProof
{
    public required string Proof { get; init; }

    public required string ProofType { get; set; } = string.Empty;
}