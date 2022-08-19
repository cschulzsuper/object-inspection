using System.Collections.Generic;

namespace Super.Paula.Application.Operation
{
    public class Extension
    {
        public string ETag { get; set; } = string.Empty;

        public string AggregateType { get; set; } = string.Empty;

        public ISet<ExtensionField> Fields { get; set; } = new HashSet<ExtensionField>();

    }
}
