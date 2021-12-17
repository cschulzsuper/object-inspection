using System.Collections.Generic;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObject
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;

        public ISet<BusinessObjectInspection> Inspections { get; set; } = new HashSet<BusinessObjectInspection>();
    }
}
