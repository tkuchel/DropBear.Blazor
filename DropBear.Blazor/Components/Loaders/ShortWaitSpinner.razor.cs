#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

public partial class ShortWaitSpinner : ComponentBase
{
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Title { get; set; } = "Short Wait";
    [Parameter] public string Message { get; set; } = "Please wait while we process your request.";

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
}
