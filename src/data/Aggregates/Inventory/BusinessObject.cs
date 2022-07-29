﻿using System.Collections.Generic;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObject
    {
        public string ETag { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;

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

        public IEnumerator<KeyValuePair<string,object>> GetEnumerator()
            => _data.GetEnumerator();
    }
}
