using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public class Extension
    {
        public string ETag { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public ISet<ExtensionField> Fields { get; set; } = new HashSet<ExtensionField>();
        
    }
}
