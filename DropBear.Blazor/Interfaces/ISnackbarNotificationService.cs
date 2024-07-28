#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface ISnackbarNotificationService
{
    event Func<SnackbarNotificationOptions,Task>? OnShow;
    event Action? OnHideAll;

    Task ShowAsync(SnackbarNotificationOptions options);

    Task ShowAsync(string message, SnackbarType type = SnackbarType.Information,
        ThemeType theme = ThemeType.DarkMode,
        int duration = 5000);

    void HideAll();
}
