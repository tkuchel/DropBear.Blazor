#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Alerts;

/// <summary>
///     A Blazor component for displaying page alerts.
/// </summary>
public sealed partial class DropBearPageAlert : DropBearComponentBase
{
    private static readonly Dictionary<AlertType, string> IconClasses = new()
    {
        { AlertType.Info, "fas fa-info-circle" },
        { AlertType.Success, "fas fa-check-circle" },
        { AlertType.Warning, "fas fa-exclamation-triangle" },
        { AlertType.Danger, "fas fa-times-circle" },
        { AlertType.Notification, "fas fa-bell" }
    };

    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public AlertType Type { get; set; } = AlertType.Info;
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public EventCallback OnClose { get; set; } = EventCallback.Empty;

#pragma warning disable CA1308
    private string AlertClassString => $"alert alert-{GetThemeClass()} alert-{Type.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308
    private string IconClassString => IconClasses[Type];

    /// <summary>
    ///     Handles the close button click event.
    /// </summary>
    private async Task OnCloseClick()
    {
        if (!IsDismissible || !OnClose.HasDelegate)
        {
            return;
        }

        await OnClose.InvokeAsync();
    }

    /// <summary>
    ///     Gets the CSS class for the theme.
    /// </summary>
    /// <returns>Returns "dark" if the theme is dark mode; otherwise, "light".</returns>
    private string GetThemeClass()
    {
        return Theme == ThemeType.DarkMode ? "dark" : "light";
    }
}
