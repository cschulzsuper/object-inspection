using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Data.Mappings.Administration;
using ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;
using ChristianSchulz.ObjectInspection.Data.Mappings.Communication;
using ChristianSchulz.ObjectInspection.Data.Mappings.Guidelines;
using ChristianSchulz.ObjectInspection.Data.Mappings.Inventory;

namespace ChristianSchulz.ObjectInspection.Data;

public class ApplicationContext : ObjectInspectionContext
{
    private readonly ExtensionProvider _extensions;

    public ApplicationContext(
        DbContextOptions<ApplicationContext> options,
        ObjectInspectionContextState state,
        ExtensionProvider extensions)

        : base(options, state)
    {
        _extensions = extensions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (!string.IsNullOrWhiteSpace(State.CurrentOrganization))
        {
            modelBuilder.HasManualThroughput(1000);

            // Inventory
            modelBuilder.ApplyConfiguration(new BusinessObjectMapping(State, _extensions));

            // Auditing
            modelBuilder.ApplyConfiguration(new BusinessObjectInspectorMapping(State));
            modelBuilder.ApplyConfiguration(new BusinessObjectInspectionMapping(State));
            modelBuilder.ApplyConfiguration(new BusinessObjectInspectionAuditRecordMapping(State));

            // Guidelines
            modelBuilder.ApplyConfiguration(new InspectionMapping(State));

            // Administration
            modelBuilder.ApplyConfiguration(new InspectorMapping(State));

            // Communication
            modelBuilder.ApplyConfiguration(new NotificationMapping(State));

            ApplyCamelCaseJsonPropertyNames(modelBuilder);
        }
    }
}