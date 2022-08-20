using System;

namespace Super.Paula.Shared.Orchestration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AllowedSubscriberAttribute : Attribute
{
    public string Name { get; }

    public AllowedSubscriberAttribute(string name)
    {
        Name = name;
    }
}