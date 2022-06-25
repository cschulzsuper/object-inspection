using Super.Paula.Data;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.AdventureTours.Steps
{
    public class Initialization : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;

        public Initialization(PaulaAdministrationContext paulaAdministrationContext)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
        }

        public async Task ExecuteAsync()
        {
            await _paulaAdministrationContext.Database.EnsureCreatedAsync();
        }
    }
}
