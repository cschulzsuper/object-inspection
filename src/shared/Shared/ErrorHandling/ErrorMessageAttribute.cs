using System;

namespace ChristianSchulz.ObjectInspection.Shared.ErrorHandling;

[AttributeUsage(AttributeTargets.Method)]
public class ErrorMessageAttribute : Attribute
{

    public ErrorMessageAttribute(string message)
    {
        Message = message;
    }

    public string Message { get; }
}