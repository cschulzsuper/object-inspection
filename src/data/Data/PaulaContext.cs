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
        public PaulaContext(DbContextOptions<PaulaContext> options, PaulaContextState state)
            : base(options)
        {
            State = state;
        }

        public PaulaContextState State { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BusinessObjectMapping());
            modelBuilder.ApplyConfiguration(new BusinessObjectInspectionAuditMapping());
            modelBuilder.ApplyConfiguration(new IdentityMapping());
            modelBuilder.ApplyConfiguration(new InspectionMapping());
            modelBuilder.ApplyConfiguration(new InspectorMapping());
            modelBuilder.ApplyConfiguration(new IdentityInspectorMapping());
            modelBuilder.ApplyConfiguration(new NotificationMapping());
            modelBuilder.ApplyConfiguration(new OrganizationMapping());
        }
    }
}