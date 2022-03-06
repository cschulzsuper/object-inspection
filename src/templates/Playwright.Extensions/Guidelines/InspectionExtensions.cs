using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Guidelines
{
    public static class InspectionExtensions
    {
        public static async Task CreateInspectionAsync(this IPage page, string displayName, string uniqueName)
        {
            await page.GotoAsync("inspections/create");

            await page.Locator("#inspectionDisplayName").FillAsync(displayName);
            await page.Locator("#inspectionUniqueName").FillAsync(uniqueName);
            await page.Locator("#inspectionText").FillAsync(Faker.Lorem.Paragraph());
            await page.Locator("#inspectionActivated").SetCheckedAsync(true);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });

            await Task.Delay(1200);
        }
    }
}
