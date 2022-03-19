using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Administration;
using Super.Paula.Data.Mappings.Auth;

namespace Super.Paula.Data
{
    public class PaulaAdministrationContext : PaulaContext
    {
        public PaulaAdministrationContext(DbContextOptions<PaulaAdministrationContext> options, PaulaContextState state)
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
        }
    }
}