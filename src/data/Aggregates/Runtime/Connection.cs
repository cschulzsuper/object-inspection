using Super.Paula.RuntimeData;
using System.Collections.Generic;

namespace Super.Paula.Application.Runtime
{
    public class Connection : IRuntimeData
    {
        public string Correlation => $"{Realm}:{Account}";

        public string Realm { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;

        public ISet<string> Proof { get; } = new HashSet<string>();
    }
}
