﻿using System;

namespace Super.Paula.Application.Orchestration
{
    public record WorkerRegistration(Type WorkerType, string WorkerName)
    {
        public int IterationDelay { get; set; }
        public string ETag { get; set; } = string.Empty;
    }
}
