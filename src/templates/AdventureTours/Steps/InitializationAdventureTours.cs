using Microsoft.Playwright;
using Super.Paula.Environment;
using Super.Paula.Templates.Playwright.Administration;
using Super.Paula.Templates.Playwright.Guidelines;
using Super.Paula.Templates.Playwright.Inventory;
using System.Threading.Tasks;

namespace Super.Paula.Steps
{
    public class InitializationAdventureTours : IStep
    {
        private readonly AppSettings _appSettings;

        public InitializationAdventureTours(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task ExecuteAsync()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                BaseURL = _appSettings.Client
            });

            var page = await context.NewPageAsync();

            var adventureToursIdentity = "bknightingale";
            var adventureToursPassword = "barnabus";
            var adventureToursInspector = "bknightingale";

            await page.SignInAsync(
                adventureToursIdentity,
                adventureToursPassword,
                withAutomaticInspectorLogin: true);

            await page.CreateBusinessObjectAsync("Radio #1234", "radio-1234", adventureToursInspector);
            await page.CreateBusinessObjectAsync("Radio #1424", "radio-1424", adventureToursInspector);
            await page.CreateBusinessObjectAsync("GPS Tracker #2234", "gps-tracker-2234", adventureToursInspector);
            await page.CreateBusinessObjectAsync("GPS Tracker #2424", "gps-tracker-2424", adventureToursInspector);
            await page.CreateBusinessObjectAsync("Flashlight #134", "flashlight-134", adventureToursInspector);
            await page.CreateBusinessObjectAsync("Flashlight #142", "flashlight-142", adventureToursInspector);

            await page.CreateInspectionAsync("Charged", "charged");
            await page.CreateInspectionAsync("Operational", "operational");
            await page.CreateInspectionAsync("Condition", "condition");

            await page.AssignBusinessObjectInspectionAsync("radio-1234", "charged");
            await page.AssignBusinessObjectInspectionAsync("radio-1234", "operational");
            await page.AssignBusinessObjectInspectionAsync("radio-1234", "condition");

            await page.AssignBusinessObjectInspectionAsync("radio-1424", "charged");
            await page.AssignBusinessObjectInspectionAsync("radio-1424", "operational");
            await page.AssignBusinessObjectInspectionAsync("radio-1424", "condition");

            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2234", "charged");
            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2234", "operational");
            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2234", "condition");

            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2424", "charged");
            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2424", "operational");
            await page.AssignBusinessObjectInspectionAsync("gps-tracker-2424", "condition");

            await page.AssignBusinessObjectInspectionAsync("flashlight-134", "charged");
            await page.AssignBusinessObjectInspectionAsync("flashlight-134", "operational");
            await page.AssignBusinessObjectInspectionAsync("flashlight-134", "condition");

            await page.AssignBusinessObjectInspectionAsync("flashlight-142", "charged");
            await page.AssignBusinessObjectInspectionAsync("flashlight-142", "operational");
            await page.AssignBusinessObjectInspectionAsync("flashlight-142", "condition");

            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1234", "charged", "0 0 1 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1234", "operational", "0 0 1 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1234", "condition", "0 0 1 * *", "12:00:00");

            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1424", "charged", "0 0 1 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1424", "operational", "0 0 1 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("radio-1424", "condition", "0 0 1 * *", "12:00:00");

            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2234", "charged", "0 0 2 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2234", "operational", "0 0 2 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2234", "condition", "0 0 2 * *", "12:00:00");

            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2424", "charged", "0 0 2 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2424", "operational", "0 0 2 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("gps-tracker-2424", "condition", "0 0 2 * *", "12:00:00");

            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-134", "charged", "0 0 3 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-134", "operational", "0 0 3 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-134", "condition", "0 0 3 * *", "12:00:00");

            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-142", "charged", "0 0 3 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-142", "operational", "0 0 3 * *", "12:00:00");
            await page.ScheduleBusinessObjectInspectionAuditAsync("flashlight-142", "condition", "0 0 3 * *", "12:00:00");

            await page.SignOutAsync();
        }
    }
}
