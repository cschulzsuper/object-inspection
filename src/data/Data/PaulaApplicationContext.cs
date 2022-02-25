using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidelines;
using Super.Paula.Data.Mappings.Inventory;

namespace Super.Paula.Data
{
    public class PaulaApplicationContext : PaulaContext
    {
        public PaulaApplicationContext(DbContextOptions<PaulaApplicationContext> options, PaulaContextState state)
            : base(options, state)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!string.IsNullOrWhiteSpace(State.CurrentOrganization))
            {
                // Inventory
                modelBuilder.ApplyConfiguration(new BusinessObjectMapping(State));
               
                // Auditing
                modelBuilder.ApplyConfiguration(new BusinessObjectInspectionMapping(State));
                modelBuilder.ApplyConfiguration(new BusinessObjectInspectionAuditRecordMapping(State));

                // Guidelines
                modelBuilder.ApplyConfiguration(new InspectionMapping(State));

                // Administration
                modelBuilder.ApplyConfiguration(new InspectorMapping(State));

                // Communication
                modelBuilder.ApplyConfiguration(new NotificationMapping(State));
            }
        }
    }
}