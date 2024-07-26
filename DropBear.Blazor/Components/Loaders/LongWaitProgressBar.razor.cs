#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

public partial class LongWaitProgressBar : ComponentBase
{
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Title { get; set; } = "Long Wait";
    [Parameter] public string Message { get; set; } = "Please wait while we process your request.";
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public int Progress { get; set; } = 0;

    [Parameter] public EventCallback OnCancel { get; set; }

    // Get css style based on theme
    private string GetThemeCssClass()
    {
        return Theme switch
        {
            ThemeType.DarkMode => "theme-dark",
            ThemeType.LightMode => "theme-light",
            _ => "theme-dark"
        };
    }

    private async Task HandleCancelClick()
    {
        await OnCancel.InvokeAsync();
    }
}
