﻿using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Administration.Requests;

public class InitializeOrganizationRequest
{
    public string ETag { get; set; } = string.Empty;

    [Required]
    [KebabCase]
    [StringLength(140)]
    [UniqueName]
    public string Inspector { get; set; } = string.Empty;
}