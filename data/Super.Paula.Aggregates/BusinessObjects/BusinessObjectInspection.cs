﻿namespace Super.Paula.Aggregates.BusinessObjects
{
    public partial class BusinessObjectInspection
    {
        public bool Activated { get; set; } = false;
        public bool InspectionActivated { get; set; } = false;

        public string Inspection { get; set; } = string.Empty;
        public string InspectionDisplayName { get; set; } = string.Empty;
        public string InspectionText { get; set; } = string.Empty;

        public int AuditTime { get; set; }
        public int AuditDate { get; set; }
        public string AuditInspector { get; set; } = string.Empty;
        public string AuditResult { get; set; } = string.Empty;
        public string AuditAnnotation { get; set; } = string.Empty;
    }
}
