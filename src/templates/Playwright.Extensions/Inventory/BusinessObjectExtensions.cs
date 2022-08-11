using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Inventory
{
    public static class BusinessObjectExtensions
    {
        public static async Task CreateBusinessObjectAsync(this IPage page, string displayName, string uniqueName, string inspector, params string[] extensionFields)
        {   
            await page.GotoAsync("business-objects/create");

            await page.Locator("#businessObjectDisplayName").FillAsync(displayName);
            await page.Locator("#businessObjectUniqueName").FillAsync(uniqueName);
            await page.Locator("#businessObjectInspector").FillAsync(inspector);

            foreach (var extensionField in extensionFields)
            {
                await page.Locator($"#businessObject{extensionField}").FillAsync(Faker.Name.First());
            }

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });

            await Task.Delay(1200);
        }
    }
}
