#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Badges;

public partial class DropBearBadge : DropBearComponentBase
{
    [Parameter] public BadgeColor Color { get; set; } = BadgeColor.Default;
    [Parameter] public BadgeShape Shape { get; set; } = BadgeShape.Normal;
    [Parameter] public string Icon { get; set; } = string.Empty;
    [Parameter] public string Text { get; set; } = string.Empty;
    [Parameter] public string Tooltip { get; set; } = string.Empty;

    private bool ShowTooltip { get; set; }
    private string TooltipStyle { get; set; } = string.Empty;

    private string CssClass => BuildCssClass();

    private string BuildCssClass()
    {
        var cssClass = "dropbear-badge";
        cssClass += $" dropbear-badge-{Color.ToString().ToLower()}";
        cssClass += $" dropbear-badge-{Shape.ToString().ToLower()}";
        if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Icon))
        {
            cssClass += " dropbear-badge-icon-only";
        }

        return cssClass.Trim();
    }

    private void OnTooltipShow(MouseEventArgs args)
    {
        if (!string.IsNullOrEmpty(Tooltip))
        {
            ShowTooltip = true;
            TooltipStyle = $"left: {args.ClientX}px; top: {args.ClientY}px;";
            StateHasChanged();
        }
    }

    private void OnTooltipHide()
    {
        ShowTooltip = false;
        StateHasChanged();
    }
}
