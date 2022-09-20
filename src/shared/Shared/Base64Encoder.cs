using System;
using System.Text;
using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Shared;

public class Base64Encoder
{
    public static string ObjectToBase64(object @object)
    {
        var json = JsonSerializer.Serialize(@object);

        var bytes = Encoding.Default.GetBytes(json);

        return Convert.ToBase64String(bytes);
    }

    public static T Base64ToObject<T>(string value)
        => (T)Base64ToObject(value, typeof(T));

    public static object Base64ToObject(string value, Type type)
    {
        var bytes = Convert.FromBase64String(value);
        var json = Encoding.Default.GetString(bytes);

        var @object = JsonSerializer.Deserialize(json, type);

        if (@object == null)
        {
            throw new Exception();
        }

        return @object;
    }
}