using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Administration
{
    public static class InspectorExtensions
    {
        public static async Task EditInspectorAsync(this IPage page, string inspector, string identity)
        {
            await page.GotoAsync($"inspectors/{inspector}/edit");

            await page.Locator("#inspectorIdentity").FillAsync(identity);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }

        public static async Task CreateInspectorAsync(this IPage page, string inspector, string identity)
        {
            await page.GotoAsync($"inspectors/create");

            await page.Locator("#inspectorUniqueName").FillAsync(inspector);
            await page.Locator("#inspectorIdentity").FillAsync(identity);
            await page.Locator("#inspectorActivated").SetCheckedAsync(true);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }
    }
}
