using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.Administration;

public static class AuthorizationExtensions
{
    public static async Task SignInInspectorAsync(this IPage page, string? organization, string? inspector)
    {
        await page.GotoAsync($"inspectors/sign-in");
        await Task.Delay(1200);

        if (inspector != null && organization != null)
        {
            await page.Locator($"#signIn{inspector}For{organization}").ClickAsync();
            await Task.Delay(1200);
        }

        await page.WaitForSelectorAsync("#indexHeadline");
    }
}