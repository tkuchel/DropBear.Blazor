#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Alerts;

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

    private string AlertClassString => $"alert alert-{GetThemeClass()} alert-{Type.ToString().ToLower()}";
    private string IconClassString => IconClasses[Type];

    private async Task OnCloseClick()
    {
        // Check is IsDismissible is false
        if (IsDismissible is false)
        {
            return;
        }

        // Check if the OnClose event is empty
        if (OnClose.HasDelegate is false)
        {
            return;
        }

        // Invoke the OnClose event
        await OnClose.InvokeAsync();
    }

    private string GetThemeClass()
    {
        return Theme == ThemeType.DarkMode ? "dark" : "light";
    }
}
