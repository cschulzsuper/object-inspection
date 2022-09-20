using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Data.Mappings.Operation;

namespace ChristianSchulz.ObjectInspection.Data;

public class OperationContext : ObjectInspectionContext
{
    public OperationContext(DbContextOptions<OperationContext> options, ObjectInspectionContextState state)
        : base(options, state)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (!string.IsNullOrWhiteSpace(State.CurrentOrganization))
        {
            modelBuilder.HasManualThroughput(1000);

            modelBuilder.ApplyConfiguration(new ExtensionMapping(State));

            ApplyCamelCaseJsonPropertyNames(modelBuilder);
        }
    }
}