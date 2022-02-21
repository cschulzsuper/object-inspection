using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Inventory
{
    public static class PageExtensions
    {
        public static async Task AssignBusinessObjectInspectionAsync(this IPage page, string businessObject, string uniqueName)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections");

            await page.Locator("#assignmentMode").ClickAsync();
            await page.Locator($"#assign{uniqueName}").ClickAsync();
        }

        public static async Task ScheduleBusinessObjectInspectionAuditAsync(this IPage page, string businessObject, string uniqueName, string schedule, string threshold)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections/{uniqueName}/schedule");

            await page.Locator("#schedule").FillAsync(schedule);
            await page.Locator("#threshold").FillAsync(threshold);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }
    }
}
