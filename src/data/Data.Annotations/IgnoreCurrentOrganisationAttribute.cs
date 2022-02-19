using System;

namespace Super.Paula.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IgnoreCurrentOrganizationAttribute : Attribute
    {
    }
}
