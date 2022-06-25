using Super.Paula.Environment;
using Super.Paula.Templates.Playwright.Administration;
using System.Threading.Tasks;
using Microsoft.Playwright;

using Playwright = Microsoft.Playwright;
using Super.Paula.Templates.Playwright.Auth;
using Super.Paula.Templates.Playwright.AdventureTours.Environment;

namespace Super.Paula.Templates.Playwright.AdventureTours.Steps
{
    public sealed class InitializationAdministration : IStep
    {
        private readonly AppSettings _appSettings;
        private readonly AdventureToursSettings _adventureToursSettings;

        public InitializationAdministration(
            AppSettings appSettings,
            AdventureToursSettings adventureToursSettings)
        {
            _appSettings = appSettings;
            _adventureToursSettings = adventureToursSettings;
        }

        public async Task ExecuteAsync()
        {
            using var playwright = await Playwright::Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                BaseURL = _appSettings.Client
            });

            var page = await context.NewPageAsync();

            var maintainerOrganization = "super";
            var maintainerOrganizationDisplayName = "Super";
            var maintainerIdentity = _appSettings.MaintainerIdentity;
            var maintainerMailAddress = "cschulz@super.local";
            var maintainerPassword = _adventureToursSettings.MaintainerPassword;
            var maintainerInspector = _appSettings.MaintainerIdentity;

            var adventureToursOrganization = "adventure-tours";
            var adventureToursOrganizationDisplayName = "Adventure Tours";

            var adventureToursChiefIdentity = "bknightingale";
            var adventureToursChiefMailAddress = "bknightingale@adventure-tours.local";
            var adventureToursChiefPassword = _adventureToursSettings.ChiefPassword;
            var adventureToursChiefInspector = "bknightingale";

            var adventureToursDemoIdentity = _appSettings.DemoIdentity;
            var adventureToursDemoMailAddress = "fdemos@adventure-tours.local";
            var adventureToursDemoPassword = _appSettings.DemoPassword;
            var adventureToursDemoInspector = _appSettings.DemoIdentity;

            await page.RegisterAsync(
                maintainerIdentity,
                maintainerMailAddress,
                maintainerPassword);

            await page.RegisterAsync(
                adventureToursChiefIdentity,
                adventureToursChiefMailAddress,
                adventureToursChiefPassword);

            await page.RegisterAsync(
                adventureToursDemoIdentity,
                adventureToursDemoMailAddress,
                adventureToursDemoPassword);

            await page.SignInAsync(
                maintainerIdentity,
                maintainerPassword,
                withAutomaticInspectorLogin: false);

            await page.RegisterOrganizationAsync(
                maintainerOrganization,
                maintainerOrganizationDisplayName);

            await page.RegisterChiefInspectorAsync(
                maintainerOrganization,
                maintainerInspector);

            await page.SignInInspectorAsync(null, null);

            await page.CreateOrganizationAsync(
                adventureToursOrganizationDisplayName,
                adventureToursOrganization,
                adventureToursChiefInspector);

            await page.RepairOrganizationAsync(
                adventureToursOrganization);

            await page.SignOutAsync();

            await page.SignInAsync(
                maintainerIdentity,
                maintainerPassword,
                withAutomaticInspectorLogin: false);

            await page.SignInInspectorAsync(
                adventureToursOrganization,
                adventureToursChiefInspector);

            await page.EditInspectorAsync(
                adventureToursChiefInspector,
                adventureToursChiefIdentity);

            await page.CreateInspectorAsync(
                adventureToursDemoInspector,
                adventureToursDemoIdentity);

            await page.SignOutAsync();
        }
    }
}
