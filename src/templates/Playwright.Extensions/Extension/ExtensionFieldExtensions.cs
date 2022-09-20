using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Extension;

public static class ExtensionFieldExtensions
{
    public static async Task CreateExtensionFieldAsync(this IPage page, string aggregateType,
        string uniqueName, string displayName, string dataName)
    {
        await page.GotoAsync($"extensions/{aggregateType}/fields/create");

        await page.Locator("#extensionFieldUniqueName").FillAsync(uniqueName);
        await page.Locator("#extensionFieldDisplayName").FillAsync(displayName);
        await page.Locator("#extensionFieldDataName").FillAsync(dataName);
        await page.Locator("#extensionFieldDataType").FillAsync("string");

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(1200);
    }
}