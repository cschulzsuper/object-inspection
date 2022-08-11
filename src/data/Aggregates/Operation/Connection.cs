using Super.Paula.RuntimeData;
using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public class Connection : IRuntimeData
    {
        public string Correlation => Account;

        public string Account { get; set; } = string.Empty;

        public ISet<string> Proof { get; } = new HashSet<string>();
    }
}
