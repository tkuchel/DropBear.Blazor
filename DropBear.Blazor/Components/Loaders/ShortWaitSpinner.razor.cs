#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

/// <summary>
///     A Blazor component for displaying a spinner for short wait times.
/// </summary>
public partial class ShortWaitSpinner : DropBearComponentBase
{
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Title { get; set; } = "Short Wait";
    [Parameter] public string Message { get; set; } = "Please wait while we process your request.";

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
}
