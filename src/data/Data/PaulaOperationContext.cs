using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Operation;

namespace Super.Paula.Data
{
    public class PaulaOperationContext : PaulaContext
    {
        public PaulaOperationContext(DbContextOptions<PaulaOperationContext> options, PaulaContextState state)
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
            }
        }
    }
}