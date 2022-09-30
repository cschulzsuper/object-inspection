using ChristianSchulz.ObjectInspection.RuntimeData;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class Connection : IRuntimeData
{
    public string Correlation => Account;

    public required string Account { get; set; } = string.Empty;

    public ISet<ConnectionProof> Proof { get; } = new HashSet<ConnectionProof>();
}