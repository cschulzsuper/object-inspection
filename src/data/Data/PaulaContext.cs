using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidelines;
using Super.Paula.Data.Mappings.Inventory;

namespace Super.Paula.Data
{
    public class PaulaContext : DbContext
    {
        public PaulaContext(DbContextOptions options, PaulaContextState state)
            : base(options)
        {
            State = state;
        }

        public PaulaContextState State { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!string.IsNullOrWhiteSpace(State.CurrentOrganization))
            {
                modelBuilder.ApplyConfiguration(new BusinessObjectMapping(State));
                modelBuilder.ApplyConfiguration(new BusinessObjectInspectionAuditRecordMapping(State));
                modelBuilder.ApplyConfiguration(new InspectionMapping(State));
                modelBuilder.ApplyConfiguration(new InspectorMapping(State));
                modelBuilder.ApplyConfiguration(new NotificationMapping(State));
            }
        }
    }
}