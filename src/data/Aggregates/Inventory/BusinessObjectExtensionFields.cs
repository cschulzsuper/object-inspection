using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public class BusinessObjectExtensionFields
{
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