using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Administration
{
    public static class InspectorExtensions
    {
        public static async Task EditInspectorIdentityAsync(this IPage page, string inspector, string identity)
        {
            await page.GotoAsync($"inspectors/{inspector}/edit");

            await page.Locator("#inspectorIdentity").FillAsync(identity);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }
    }
}
