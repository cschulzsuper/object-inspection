using Super.Paula.Environment;
using Super.Paula.Templates.Playwright.Administration;
using System.Threading.Tasks;
using Microsoft.Playwright;

using playwright = Microsoft.Playwright;

namespace Super.Paula.Templates.AdventureTours.Steps
{
    public sealed class InitializationAdministration : IStep
    {
        private readonly AppSettings _appSettings;

        public InitializationAdministration(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            using var playwright = await playwright::Playwright.CreateAsync();
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
            var maintainerPassword = "super";
            var maintainerInspector = _appSettings.MaintainerIdentity;

            var adventureToursOrganization = "adventure-tours";
            var adventureToursOrganizationDisplayName = "Adventure Tours";
            
            var adventureToursMaintainerIdentity = "bknightingale";
            var adventureToursMaintainerMailAddress = "bknightingale@adventure-tours.local";
            var adventureToursMaintainerPassword = "barnabus";
            var adventureToursMaintainerInspector = "bknightingale";

            var adventureToursDemoIdentity = _appSettings.DemoIdentity;
            var adventureToursDemoMailAddress = "fdemos@adventure-tours.local";
            var adventureToursDemoPassword = _appSettings.DemoPassword;
            var adventureToursDemoInspector = _appSettings.DemoIdentity;

            await page.RegisterAsync(
                maintainerIdentity,
                maintainerMailAddress,
                maintainerPassword);

            await page.RegisterAsync(
                adventureToursMaintainerIdentity,
                adventureToursMaintainerMailAddress,
                adventureToursMaintainerPassword);

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
                maintainerInspector,
                withAutomaticInspectorLogin: true);

            await page.CreateOrganizationAsync(
                adventureToursOrganizationDisplayName,
                adventureToursOrganization,
                adventureToursMaintainerInspector);

            await page.RepairOrganizationAsync(
                adventureToursOrganization);

            await page.SignOutAsync();

            await page.SignInAsync(
                maintainerIdentity,
                maintainerPassword,
                withAutomaticInspectorLogin: false);

            await page.SignInInspectorAsync(
                adventureToursOrganization,
                adventureToursMaintainerInspector);

            await page.EditInspectorAsync(
                adventureToursMaintainerInspector,
                adventureToursMaintainerIdentity);

            await page.CreateInspectorAsync(
                adventureToursDemoInspector,
                adventureToursDemoIdentity);

            await page.SignOutAsync();
        }
    }
}
