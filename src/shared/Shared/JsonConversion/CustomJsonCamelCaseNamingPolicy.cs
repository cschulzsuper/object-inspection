using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Shared.JsonConversion;

public class CustomJsonCamelCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => CaseStyleConverter.FromPascalCaseToCamelCase(name);
}