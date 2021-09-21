using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Super.Paula.ErrorHandling
{
    public class ProblemDetails
    {
        public ProblemDetails()
        {
        }

        public ProblemDetails(IDictionary<string, FormattableString[]>? errors)
        {
            if (errors != null)
            {
                Errors = errors.ToDictionary(
                    x => x.Key,
                    x => x.Value
                    .Select(y => y.ToString())
                    .ToArray());
            }
        }

        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int? Status { get; set; }

        public string Detail { get; set; } = string.Empty;

        public string Instance { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object>? Extensions { get; init; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, string[]>? Errors { get; } = null;
    }
}