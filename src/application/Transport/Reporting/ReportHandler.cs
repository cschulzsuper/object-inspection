using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Reporting.Responses;
using ChristianSchulz.ObjectInspection.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Reporting
{
    public class ReportHandler : IReportHandler
    {
        private readonly IBusinessObjectInspectionAuditRecordRequestHandler _businessObjectInspectionAuditRecordRequestHandler;

        public ReportHandler(IBusinessObjectInspectionAuditRecordRequestHandler businessObjectInspectionAuditRecordRequestHandler)
        {
            _businessObjectInspectionAuditRecordRequestHandler=businessObjectInspectionAuditRecordRequestHandler;
        }



        public async ValueTask<BusinessObjectInspectionAuditRecordResultReportResponse> GetBusinessObjectInspectionAuditRecordResultReportAsync(string query)
        {
            var globalNow = DateTimeNumbers.GlobalNow;

            var response = new BusinessObjectInspectionAuditRecordResultReportResponse
            {
                ReportDate = globalNow.Date,
                ReportTime = globalNow.Time,
                Records = new HashSet<BusinessObjectInspectionAuditRecordResultReportRecordResponse>()
            };

            var records = _businessObjectInspectionAuditRecordRequestHandler.GetAll(query,0, int.MaxValue);

            await foreach(var record in records)
            {
                var existingRecordForResult = response.Records
                    .SingleOrDefault(x =>
                        x.AuditDate == record.AuditDate &&
                        x.Result == record.Result);

                if(existingRecordForResult != null)
                {
                    existingRecordForResult.Percentag += 1;
                }
                else
                {
                    response.Records.Add(new BusinessObjectInspectionAuditRecordResultReportRecordResponse
                    {
                        AuditDate = record.AuditDate,
                        Percentag= 1,
                        Result = record.Result
                    });
                }
            }

            var recordsByAuditDate = response.Records.GroupBy(x => x.AuditDate);

            foreach( var recordsForAuditDate in recordsByAuditDate)
            {
                var precentageSum = recordsForAuditDate.Sum(x => x.Percentag);
                
                foreach( var record in recordsForAuditDate)
                {
                    record.Percentag = precentageSum == default 
                        ? default 
                        : (record.Percentag / precentageSum);
                }
            }

            return response;
        }
    }
}
