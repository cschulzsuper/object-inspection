using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class Initialization : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;

        public Initialization(PaulaAdministrationContext paulaAdministrationContext)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
        }

        public Task ExecuteAsync()
        {
            _paulaAdministrationContext.Database.EnsureCreated();

            return Task.CompletedTask;
        }
    }
}
