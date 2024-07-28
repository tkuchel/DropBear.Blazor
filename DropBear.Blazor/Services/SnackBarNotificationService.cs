#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

public sealed class SnackbarNotificationService : ISnackbarNotificationService
{
    public event Func<SnackbarNotificationOptions,Task>? OnShow;
    public event Action? OnHideAll;

    public Task ShowAsync(string message, SnackbarType type = SnackbarType.Information,
        ThemeType theme = ThemeType.DarkMode,
        int duration = 5000)
    {
        var options =
            new SnackbarNotificationOptions { Message = message, Type = type, Theme = theme, Duration = duration };
        return ShowAsync(options);
    }

    public Task ShowAsync(SnackbarNotificationOptions options)
    {
        OnShow?.Invoke(options);
        return Task.CompletedTask;
    }

    public void HideAll()
    {
        OnHideAll?.Invoke();
    }
}
