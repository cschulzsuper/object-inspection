using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Super.Paula.Templates.Playwright.Auth
{
    public static class AuthenticationExtensions
    {
        public static async Task RegisterAsync(this IPage page, string uniqueName, string mailAddress, string password)
        {
            await page.GotoAsync("register");

            await page.Locator("#identityUniqueName").FillAsync(uniqueName);
            await page.Locator("#identityMailAddress").FillAsync(mailAddress);
            await page.Locator("#identitySecret").FillAsync(password);

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("#submit").ClickAsync();
            });

            await Task.Delay(1200);
        }

        public static async Task SignInAsync(this IPage page, string identity, string password, bool withAutomaticInspectorLogin)
        {
            await page.GotoAsync(string.Empty);

            await page.Locator("#identityUniqueName").FillAsync(identity);
            await page.Locator("#identitySecret").FillAsync(password);

            await page.Locator("#submit").ClickAsync();

            await Task.Delay(1200);

            await (!withAutomaticInspectorLogin
                ? page.WaitForSelectorAsync("#signInInspectorHeadline")
                : page.WaitForSelectorAsync("#indexHeadline"));
        }

        public static async Task SignOutAsync(this IPage page)
        {
            await Task.Delay(1200);

            await page.Locator("#signOut").ClickAsync();
            await page.WaitForSelectorAsync("#signInHeadline");
        }
    }
}
