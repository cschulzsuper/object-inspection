using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Administration;

public static class OrganizationExtensions
{
    public static async Task CreateOrganizationAsync(this IPage page, string displayName, string uniqueName, string chiefInspector)
    {
        await page.GotoAsync("organizations/create");

        await page.Locator("#organizationDisplayName").FillAsync(displayName);
        await page.Locator("#organizationUniqueName").FillAsync(uniqueName);
        await page.Locator("#organizationChiefInspector").FillAsync(chiefInspector);
        await page.Locator("#organizationActivated").CheckAsync();

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(1200);
    }

    public static async Task RepairOrganizationAsync(this IPage page, string organization)
    {
        await page.GotoAsync($"organizations/{organization}/inspectors");

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#repair").ClickAsync();
        });

        await Task.Delay(1200);
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

    public static async Task RegisterChiefInspectorAsync(this IPage page, string organization, string inspector)
    {
        await page.GotoAsync($"register-chief-inspector/{organization}");

        await page.Locator("#inspector").FillAsync(inspector);

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(4_800);
    }
}