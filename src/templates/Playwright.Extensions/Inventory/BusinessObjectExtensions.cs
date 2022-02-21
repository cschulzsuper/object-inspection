using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Inventory
{
    public static class BusinessObjectExtensions
    {
        public static async Task CreateBusinessObjectAsync(this IPage page, string displayName, string uniqueName, string inspector)
        {   
            await page.GotoAsync("business-objects/create");

            await page.Locator("#businessObjectDisplayName").FillAsync(displayName);
            await page.Locator("#businessObjectUniqueName").FillAsync(uniqueName);
            await page.Locator("#businessObjectInspector").FillAsync(inspector);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }
    }
}
