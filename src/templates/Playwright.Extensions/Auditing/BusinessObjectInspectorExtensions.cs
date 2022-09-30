using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Auditing;

public static class BusinessObjectInspectorExtensions
{
    public static async Task CreateBusinessObjectInspectorAsync(this IPage page, string businessObject, string inspector)
    {
        await page.GotoAsync($"business-objects/{businessObject}/inspectors");

        await page.Locator($"#assign-{inspector}").ClickAsync();

        await Task.Delay(1200);
    }

    public static async Task DeleteBusinessObjectInspectorAsync(this IPage page, string businessObject, string inspector)
    {
        await page.GotoAsync($"business-objects/{businessObject}/inspectors");

        await page.Locator($"#unassign-{inspector}").ClickAsync();

        await Task.Delay(1200);
    }
}