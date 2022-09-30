using System;

namespace ChristianSchulz.ObjectInspection.Application;

[AttributeUsage(AttributeTargets.Method)]
public class UseOrganizationFromRouteAttribute : Attribute
{
}