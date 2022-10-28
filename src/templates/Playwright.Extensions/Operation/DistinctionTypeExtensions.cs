using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Extension;

public static class DistinctionTypeExtensions
{
    public static async Task CreateDistinctionTypeAsync(this IPage page, string uniqueName)
    {
        await page.GotoAsync("distinction-types/create");

        await page.Locator("#distinctionTypeUniqueName").FillAsync(uniqueName);

        await page.RunAndWaitForNavigationAsync(async () =>
        {
            await page.Locator("#submit").ClickAsync();
        });

        await Task.Delay(1200);
    }
}