using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Auditing
{
    public static class PageExtensions
    {
        public static async Task CreateBusinessObjectInspectionAsync(this IPage page, string businessObject, string inspection)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections");

            await page.Locator("#assignmentMode").ClickAsync();
            await page.Locator($"#assign-{inspection}").ClickAsync();
        }

        public static async Task DeleteBusinessObjectInspectionAsync(this IPage page, string businessObject, string inspection)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections");

            await page.Locator("#assignmentMode").ClickAsync();
            await page.Locator($"#unassign-{inspection}").ClickAsync();
        }

        public static async Task ScheduleBusinessObjectInspectionAuditAsync(this IPage page, string businessObject, string inspection, string schedule, string threshold)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections/{inspection}/schedule");

            await page.Locator("#schedule").FillAsync(schedule);
            await page.Locator("#threshold").FillAsync(threshold);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }

        public static async Task AuditBusinessObjectInspectionAsync(this IPage page, string businessObject, string inspection)
        {
            await page.GotoAsync($"business-objects/{businessObject}/audit");
            await page.Locator($"#audit-{businessObject}-{inspection}").ClickAsync();
        }

        public static async Task OmitBusinessObjectInspectionAuditAsync(this IPage page, string businessObject, string inspection)
        {
            await page.GotoAsync($"business-objects/{businessObject}/inspections");
            await page.Locator($"#drop-{inspection}").ClickAsync();
        }

        public static async Task AnnotateBusinessObjectInspectionAuditAsync(this IPage page, string businessObject, string inspection)
        {
            await page.GotoAsync($"/business-objects/{businessObject}/inspections/{inspection}/annotation");
            await page.Locator($"#auditAnnotation").FillAsync(Faker.Lorem.Paragraph());

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });
        }
    }
}
