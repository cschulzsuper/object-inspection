using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mapping;
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
            modelBuilder.ApplyConfiguration(new InspectionMapping());
            modelBuilder.ApplyConfiguration(new InspectorMapping());
            modelBuilder.ApplyConfiguration(new OrganizationMapping());
        }
    }
}