#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

/// <summary>
///     A Blazor component for displaying a progress bar for long wait times.
/// </summary>
public sealed partial class LongWaitProgressBar : DropBearComponentBase
{
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Title { get; set; } = "Long Wait";
    [Parameter] public string Message { get; set; } = "Please wait while we process your request.";
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public int Progress { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    /// <summary>
    ///     Gets the CSS class based on the selected theme.
    /// </summary>
    /// <returns>A string representing the CSS class.</returns>
    private string GetThemeCssClass()
    {
        return Theme switch
        {
            ThemeType.DarkMode => "theme-dark",
            ThemeType.LightMode => "theme-light",
            _ => "theme-dark"
        };
    }

    /// <summary>
    ///     Handles the cancel button click event.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandleCancelClick()
    {
        await OnCancel.InvokeAsync();
    }
}
