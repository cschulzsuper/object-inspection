using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Administration
{
    public static class AccountExtensions
    {
        public static async Task RepairOrganizationAsync(this IPage page, string organization)
        {
            await page.GotoAsync($"organizations/{organization}/inspectors");

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#repair").ClickAsync();
            });

            await Task.Delay(1200);
        }

        public static async Task SignInInspectorAsync(this IPage page, string? organization, string? inspector)
        {
            await page.GotoAsync($"sign-in-inspector");
            await Task.Delay(1200);

            if (inspector != null && organization != null)
            {
                await page.Locator($"#signIn{inspector}For{organization}").ClickAsync();
                await Task.Delay(1200);
            }

            await page.WaitForSelectorAsync("#indexHeadline");
        }

        public static async Task RegisterOrganizationAsync(this IPage page, string uniqueName, string displayName)
        {
            await page.GotoAsync("register-organization");

            await page.Locator("#organizationUniqueName").FillAsync(uniqueName);
            await page.Locator("#organizationDisplayName").FillAsync(displayName);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });

            await Task.Delay(12_000);
        }

        public static async Task RegisterChiefInspectorAsync(this IPage page, string organization, string inspector, string identity, bool withAutomaticInspectorLogin)
        {
            await page.GotoAsync($"register-chief-inspector/{organization}");

            await page.Locator("#inspector").FillAsync(inspector);
            await page.Locator("#identity").FillAsync(identity);

            await page.Locator("#submit").ClickAsync();

            await Task.Delay(1200);

            await (!withAutomaticInspectorLogin
                ? page.WaitForSelectorAsync("#signInInspectorHeadline")
                : page.WaitForSelectorAsync("#indexHeadline"));
        }
    }
}
