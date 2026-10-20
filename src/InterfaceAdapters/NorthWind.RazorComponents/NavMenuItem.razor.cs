using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace NorthWind.RazorComponents;

public partial class NavMenuItem
{
    [Parameter]
    public string Href { get; set; } = string.Empty;

    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    [Parameter]
    public string? IconClass { get; set; }

    [Parameter]
    public string? BadgeText { get; set; }

    [Parameter]
    public string BadgeClass { get; set; } = "badge-subtle-primary";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
