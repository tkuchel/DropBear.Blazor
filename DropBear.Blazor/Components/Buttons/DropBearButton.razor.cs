#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Buttons;

public sealed partial class DropBearButton : DropBearComponentBase
{
    [Parameter] public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Solid;
    [Parameter] public ButtonColor Color { get; set; } = ButtonColor.Default;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Medium;
    [Parameter] public bool IsBlock { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string Icon { get; set; } = string.Empty;
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = new();

    private string CssClass => BuildCssClass();

    private string BuildCssClass()
    {
        var cssClass = "dropbear-btn";
        cssClass += $" dropbear-btn-{ButtonStyle.ToString().ToLower()}";
        cssClass += $" dropbear-btn-{Color.ToString().ToLower()}";
        cssClass += $" dropbear-btn-{Size.ToString().ToLower()}";

        if (IsBlock)
        {
            cssClass += " dropbear-btn-block";
        }

        if (IsDisabled)
        {
            cssClass += " dropbear-btn-disabled";
        }

        if (!string.IsNullOrEmpty(Icon) && ChildContent == null)
        {
            cssClass += " dropbear-btn-icon-only";
        }

        return cssClass.Trim();
    }

    private async Task OnClickHandler(MouseEventArgs args)
    {
        if (!IsDisabled && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
