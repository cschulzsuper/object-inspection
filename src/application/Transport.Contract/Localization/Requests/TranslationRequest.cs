﻿namespace ChristianSchulz.ObjectInspection.Application.Localization.Requests;

public class TranslationRequest
{
    public string? Category { get; set; }

    public string Hash { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}