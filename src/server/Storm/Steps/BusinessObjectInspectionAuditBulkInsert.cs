using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Inventory;
using Super.Paula.Data;
using Super.Paula.Environment;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Steps
{
    public class BusinessObjectInspectionAuditBulkInsert : IStep
    {
        private readonly IBusinessObjectInspectionAuditManager _businessObjectInspectionAuditManager;

        public BusinessObjectInspectionAuditBulkInsert(
            IBusinessObjectInspectionAuditManager businessObjectInspectionAuditManager, AppState appState)
        {
            appState.CurrentOrganization = "super";
            _businessObjectInspectionAuditManager = businessObjectInspectionAuditManager;
        }

        public async Task ExecuteAsync()
        {
            for (var i = 2000000; i < 9000000; i++)
            {
                var businessObjectInspectionAudit = new BusinessObjectInspectionAudit
                {
                    Annotation = $"{Guid.NewGuid()}",
                    BusinessObjectDisplayName = $"{Guid.NewGuid()}",
                    InspectionDisplayName = $"{Guid.NewGuid()}",
                    Inspection = $"inspection-{i%5}",
                    BusinessObject = $"business-object-{i%6}",
                    Inspector = $"inspector-{i%6}",
                    AuditTime = i%100,
                    AuditDate = i/100,
                    Result = "failed"
                };

                await _businessObjectInspectionAuditManager.InsertAsync(businessObjectInspectionAudit);
            }
        }
    }
}
