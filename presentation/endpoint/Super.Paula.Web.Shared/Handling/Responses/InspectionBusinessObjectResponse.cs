﻿namespace Super.Paula.Web.Shared.Handling.Responses
{
    public class InspectionBusinessObjectResponse
    {
        public string BusinessObject { get; set; } = string.Empty;
        public bool InspectionActivated { get; set; }
        public string InspectionDisplayName { get; set; } = string.Empty;
        public string InspectionText { get; set; } = string.Empty;
        public string Inspection { get; set; } = string.Empty;
    }
}