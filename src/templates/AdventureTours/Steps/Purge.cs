using Super.Paula.Data;
using System.Threading.Tasks;

namespace Super.Paula.Templates.AdventureTours.Steps
{
    public class Purge : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;

        public Purge(PaulaAdministrationContext paulaAdministrationContext)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
        }

        public async Task ExecuteAsync()
        {
            await _paulaAdministrationContext.Database.EnsureDeletedAsync();
        }
    }
}
