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

            var maintainerIdentity = _appSettings.MaintainerIdentity;
            var maintainerMailAddress = "cschulz@super.local";
            var maintainerPassword = "super";
            var maintainerInspector = "cschulzsuper";
            var maintainerOrganization = "super";
            var maintainerOrganizationDisplayName = "Super";

            var adventureToursIdentity = "bknightingale";
            var adventureToursMailAddress = "bknightingale@adventure-tours.local";
            var adventureToursPassword = "barnabus";
            var adventureToursInspector = "bknightingale";
            var adventureToursOrganization = "adventure-tours";
            var adventureToursOrganizationDisplayName = "Adventure Tours";

            await page.RegisterAsync(
                maintainerIdentity,
                maintainerMailAddress,
                maintainerPassword);

            await page.RegisterAsync(
                adventureToursIdentity,
                adventureToursMailAddress,
                adventureToursPassword);

            await page.SignInAsync(
                maintainerIdentity,
                maintainerPassword,
                withAutomaticInspectorLogin: false);

            await page.RegisterOrganizationAsync(
                maintainerOrganization,
                maintainerOrganizationDisplayName);

            await Task.Delay(6000);

            await page.RegisterChiefInspectorAsync(
                maintainerOrganization,
                maintainerInspector,
                maintainerIdentity,
                withAutomaticInspectorLogin: false);

            await page.SignInInspectorAsync(null, null);

            await page.CreateOrganizationAsync(
                adventureToursOrganizationDisplayName,
                adventureToursOrganization,
                adventureToursInspector);

            await Task.Delay(6000);

            await page.RepairOrganizationAsync(
                adventureToursOrganization);

            await Task.Delay(6000);

            await page.SignOutAsync();

            await page.SignInAsync(
                maintainerIdentity,
                maintainerPassword,
                withAutomaticInspectorLogin: false);

            await page.SignInInspectorAsync(
                adventureToursOrganization,
                adventureToursInspector);

            await page.EditInspectorIdentityAsync(
                adventureToursInspector,
                adventureToursIdentity);

            await page.SignOutAsync();
        }
    }
}
