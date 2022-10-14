using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public class BusinessObject
{
    public long Id { get; set; }
    public string ETag { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public string DistinctionType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public BusinessObjectExtensionFields ExtensionFields { get; set; } = new BusinessObjectExtensionFields();
}