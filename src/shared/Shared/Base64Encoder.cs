using System;
using System.Text;
using System.Text.Json;

namespace Super.Paula.Shared;

public class Base64Encoder
{
    public static string ObjectToBase64(object @object)
    {
        var json = JsonSerializer.Serialize(@object);

        var coded = (char)0x00 + json;
        var bytes = Encoding.Default.GetBytes(coded);

        return Convert.ToBase64String(bytes);
    }

    public static T Base64ToObject<T>(string value)
        => (T)Base64ToObject(value, typeof(T));

    public static object Base64ToObject(string value, Type type)
    {
        var bytes = Convert.FromBase64String(value);
        var json = Encoding.Default.GetString(bytes);

        json = json.TrimStart((char)0x00);

        var @object = JsonSerializer.Deserialize(json, type);

        if (@object == null)
        {
            throw new Exception();
        }

        return @object;
    }
}