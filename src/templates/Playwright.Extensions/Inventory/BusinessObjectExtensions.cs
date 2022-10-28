using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Inventory;

public static class BusinessObjectExtensions
{
    public static async Task CreateBusinessObjectAsync(this IPage page, 
        string displayName, string uniqueName, string distinctionType,
        params string[] extensionFields)
    {
        await page.GotoAsync("business-objects/create");

        await page.Locator("#businessObjectDistinctionType").FillAsync(distinctionType);
        await page.Locator("#businessObjectDisplayName").FillAsync(displayName);
        await page.Locator("#businessObjectUniqueName").FillAsync(uniqueName);

        await Task.Delay(100);

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