using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Data.Mappings.Administration;
using ChristianSchulz.ObjectInspection.Data.Mappings.Authentication;
using ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

namespace ChristianSchulz.ObjectInspection.Data;

public class AdministrationContext : ObjectInspectionContext
{
    public AdministrationContext(DbContextOptions<AdministrationContext> options, ObjectInspectionContextState state)
        : base(options, state)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasManualThroughput(1000);

        modelBuilder.ApplyConfiguration(new IdentityMapping());
        modelBuilder.ApplyConfiguration(new IdentityInspectorMapping());
        modelBuilder.ApplyConfiguration(new OrganizationMapping());

        modelBuilder.ApplyConfiguration(new ContinuationMapping());
        modelBuilder.ApplyConfiguration(new EventMapping());
        modelBuilder.ApplyConfiguration(new EventProcessingMapping());
        modelBuilder.ApplyConfiguration(new WorkerMapping());

        ApplyCamelCaseJsonPropertyNames(modelBuilder);
    }
}