using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Extension;

public static class ExtensionExtensions
{
    public static async Task CreateExtensionAsync(this IPage page, string aggregateType)
    {
        await page.GotoAsync("extensions/create");

        await page.Locator("#extensionAggregateType").FillAsync(aggregateType);

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(1200);
    }
}