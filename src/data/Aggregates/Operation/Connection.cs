using Super.Paula.RuntimeData;
using System.Collections.Generic;

namespace Super.Paula.Application.Operation;

public class Connection : IRuntimeData
{
    public string Correlation => Account;

    public required string Account { get; set; } = string.Empty;

    public ISet<ConnectionProof> Proof { get; } = new HashSet<ConnectionProof>();
}