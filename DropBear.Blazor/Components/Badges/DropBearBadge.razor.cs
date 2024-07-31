#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Badges;

/// <summary>
///     A Blazor component for displaying badges with optional tooltips.
/// </summary>
public sealed partial class DropBearBadge : DropBearComponentBase
{
    [Parameter] public BadgeColor Color { get; set; } = BadgeColor.Default;
    [Parameter] public BadgeShape Shape { get; set; } = BadgeShape.Normal;
    [Parameter] public string Icon { get; set; } = string.Empty;
    [Parameter] public string Text { get; set; } = string.Empty;
    [Parameter] public string Tooltip { get; set; } = string.Empty;

    private bool ShowTooltip { get; set; }
    private string TooltipStyle { get; set; } = string.Empty;

    private string CssClass => BuildCssClass();

    /// <summary>
    ///     Builds the CSS class for the badge based on its properties.
    /// </summary>
    /// <returns>A string representing the CSS class.</returns>
    private string BuildCssClass()
    {
        var cssClass = "dropbear-badge";
#pragma warning disable CA1308
        cssClass += $" dropbear-badge-{Color.ToString().ToLowerInvariant()}";
        cssClass += $" dropbear-badge-{Shape.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308
        if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Icon))
        {
            cssClass += " dropbear-badge-icon-only";
        }

        return cssClass.Trim();
    }

    /// <summary>
    ///     Shows the tooltip at the specified mouse position.
    /// </summary>
    /// <param name="args">The mouse event arguments.</param>
    private void OnTooltipShow(MouseEventArgs args)
    {
        if (string.IsNullOrEmpty(Tooltip))
        {
            return;
        }

        ShowTooltip = true;
        TooltipStyle = $"left: {args.ClientX}px; top: {args.ClientY}px;";
        StateHasChanged();
    }

    /// <summary>
    ///     Hides the tooltip.
    /// </summary>
    private void OnTooltipHide()
    {
        ShowTooltip = false;
        StateHasChanged();
    }
}
