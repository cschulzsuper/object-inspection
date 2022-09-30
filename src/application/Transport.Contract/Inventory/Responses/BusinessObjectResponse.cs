using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChristianSchulz.ObjectInspection.Application.Inventory.Responses;

[JsonConverter(typeof(BusinessObjectResponseJsonConverter))]
public class BusinessObjectResponse
{
    public string UniqueName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string ETag { get; set; } = string.Empty;

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