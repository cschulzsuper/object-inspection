using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Client.Components.Forms;

public class AuthorizedButton : ComponentBase
{
    [Parameter]
    public string Policy { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = null!;

    [Inject]
    public IAuthorizationService AuthorizationService { get; set; } = null!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    private bool _isAuthorized = false;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthenticationState).User;
        _isAuthorized = await IsAuthorizedAsync(user);
    }


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "disabled", !_isAuthorized);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();

        base.BuildRenderTree(builder);
    }

    private async Task<bool> IsAuthorizedAsync(ClaimsPrincipal user)
    {
        var result = await AuthorizationService.AuthorizeAsync(user, Policy);
        return result.Succeeded;
    }
}