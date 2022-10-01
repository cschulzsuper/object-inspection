using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Inventory.Requests;

[JsonConverter(typeof(BusinessObjectRequestJsonConverter))]
public class BusinessObjectRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string DistinctionType { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string DisplayName { get; set; } = string.Empty;

    private readonly IDictionary<string, object> _data = new Dictionary<string, object>();

    public object? this[string key]
    {
        get
        {
            return _data.TryGetValue(key, out var value)
                ? value
                : null;
        }

        set
        {
            if (value != null)
            {
                _data[key] = value;
            }
        }
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        => _data.GetEnumerator();
}