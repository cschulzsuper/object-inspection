﻿namespace ChristianSchulz.ObjectInspection.Application.Administration.Responses;

public class OrganizationResponse
{
    public string ETag { get; set; } = string.Empty;
    public string ChiefInspector { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
    public bool Activated { get; set; }
}