using ChristianSchulz.ObjectInspection.Application.Reporting.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Reporting
{
    public interface IReportHandler
    {
        ValueTask<BusinessObjectInspectionAuditRecordResultReportResponse> GetBusinessObjectInspectionAuditRecordResultReportAsync(string query);
    }
}
