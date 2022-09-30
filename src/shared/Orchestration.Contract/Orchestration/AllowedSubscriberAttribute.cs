using System;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AllowedSubscriberAttribute : Attribute
{
    public string Name { get; }

    public AllowedSubscriberAttribute(string name)
    {
        Name = name;
    }
}