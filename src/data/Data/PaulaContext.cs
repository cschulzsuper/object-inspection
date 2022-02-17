using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auditing;
using Super.Paula.Data.Mappings.Communication;
using Super.Paula.Data.Mappings.Guidelines;
using Super.Paula.Data.Mappings.Inventory;
using Super.Paula.Environment;

namespace Super.Paula.Data
{
    public class PaulaContext : DbContext
    {
        public PaulaContext(DbContextOptions<PaulaContext> options, AppState appState)
            : base(options)  
        {
            AppState = appState;
        }

        public AppState AppState { get; }

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