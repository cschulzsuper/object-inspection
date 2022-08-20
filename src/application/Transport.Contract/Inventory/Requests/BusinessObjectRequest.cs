using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Inventory.Requests;

[JsonConverter(typeof(BusinessObjectRequestJsonConverter))]
public class BusinessObjectRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string UniqueName { get; set; } = string.Empty;

    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspector { get; set; } = string.Empty;

    [Required]
    [StringLength(140)]
    public string DisplayName { get; set; } = string.Empty;

    private IDictionary<string, object> _data = new Dictionary<string, object>();

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