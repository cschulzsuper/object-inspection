using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class Reset : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;

        public Reset(PaulaAdministrationContext paulaAdministrationContext)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
        }

        public Task ExecuteAsync()
        {
            _paulaAdministrationContext.Database.EnsureDeleted();
            _paulaAdministrationContext.Database.EnsureCreated();

            return Task.CompletedTask;
        }
    }
}
