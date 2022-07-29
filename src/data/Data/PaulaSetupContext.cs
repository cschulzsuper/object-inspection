using Microsoft.EntityFrameworkCore;
using Super.Paula.Data.Mappings.Setup;

namespace Super.Paula.Data
{
    public class PaulaSetupContext : PaulaContext
    {
        public PaulaSetupContext(DbContextOptions<PaulaSetupContext> options, PaulaContextState state)
            : base(options, state)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasManualThroughput(1000);

            modelBuilder.ApplyConfiguration(new ExtensionMapping(State));
        }
    }
}