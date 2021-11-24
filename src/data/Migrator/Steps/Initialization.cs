using Migrator;
using Super.Paula.Data;
using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class Initialization : IStep
    {
        private readonly PaulaContext _paulaContext;

        public Initialization(PaulaContext paulaContext)
        {
            _paulaContext = paulaContext;
        }

        public Task ExecuteAsync()
        {
            _paulaContext.Database.EnsureCreated();

            return Task.CompletedTask;
        }
    }
}
