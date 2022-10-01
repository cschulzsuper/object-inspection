using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Extension;

public static class DistinctionTypeFieldExtensions
{
    public static async Task CreateDistinctionTypeFieldAsync(this IPage page, string uniqueName, string extensionField)
    {
        await page.GotoAsync($"distinction-types/{uniqueName}/fields/create");

        await page.Locator("#distinctionTypeFieldExtensionField").FillAsync(extensionField);

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(1200);
    }
}