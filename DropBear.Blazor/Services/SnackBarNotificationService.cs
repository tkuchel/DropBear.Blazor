#region

using System.Diagnostics;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

public sealed class SnackbarNotificationService : ISnackbarNotificationService
{
    public event Action? OnHideAll;

    public void HideAll()
    {
        OnHideAll?.Invoke();
    }

    public event Func<SnackbarNotificationOptions, Task>? OnShow;

    public Task ShowAsync(string message, SnackbarType type = SnackbarType.Information,
        ThemeType theme = ThemeType.DarkMode, int duration = 5000)
    {
        var options = new SnackbarNotificationOptions
        {
            Message = message, Type = type, Theme = theme, Duration = duration
        };
        return ShowAsync(options);
    }

    public async Task ShowAsync(SnackbarNotificationOptions options)
    {
        try
        {
            if (OnShow != null)
            {
                await OnShow.Invoke(options);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as appropriate for your application
            Debug.WriteLine($"Error showing snackbar: {ex.Message}");
        }
    }
}
