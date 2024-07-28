#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

public class SnackbarNotificationOptions
{
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public SnackbarType Type { get; init; } = SnackbarType.Information;
    public ThemeType Theme { get; init; } = ThemeType.DarkMode;
    public int Duration { get; init; } = 5000;
    public bool IsDismissible { get; init; } = true;
    public string ActionText { get; init; } = "Dismiss";
    public Func<Task> OnAction { get; init; } = () => Task.CompletedTask;
}
