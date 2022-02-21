using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Administration
{
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
        }
    }
}
