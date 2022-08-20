using System;
using System.Text.Json;

namespace Super.Paula.Shared.JsonConversion;

public class CustomJsonCamelCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => CaseStyleConverter.FromPascalCaseToCamelCase(name);
}