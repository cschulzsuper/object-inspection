using System;

namespace ChristianSchulz.ObjectInspection.Shared.SignalR;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HubNameAttribute : Attribute
{
    public string Name { get; private set; }

    public HubNameAttribute(string name)
    {
        Name = name;
    }
}