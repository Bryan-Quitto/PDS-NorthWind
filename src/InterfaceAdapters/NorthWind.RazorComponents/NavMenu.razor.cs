using Microsoft.AspNetCore.Components;

namespace NorthWind.RazorComponents;

public partial class NavMenu
{
    [Parameter]
    public string Title { get; set; } = "NorthWind";

    [Parameter]
    public string Subtitle { get; set; } = "Enterprise POS";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    private bool _collapseNavMenu = true;

    private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }
}
