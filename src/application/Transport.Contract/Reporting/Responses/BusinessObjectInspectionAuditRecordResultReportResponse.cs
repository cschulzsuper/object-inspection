using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Reporting.Responses
{
    public class BusinessObjectInspectionAuditRecordResultReportResponse
    {
        public required int ReportDate { get; set; }
        public required int ReportTime { get; set; }
        public required ISet<BusinessObjectInspectionAuditRecordResultReportRecordResponse> Records { get; set; }
    }
}
