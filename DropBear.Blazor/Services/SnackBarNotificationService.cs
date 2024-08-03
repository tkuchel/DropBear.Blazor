#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Exceptions;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

/// <summary>
///     Service to manage snackbar notifications.
/// </summary>
public sealed class SnackbarNotificationService : ISnackbarNotificationService
{
    /// <summary>
    ///     Event triggered when a new snackbar should be shown.
    /// </summary>
    public event EventHandler<SnackbarNotificationEventArgs>? OnShow;

    /// <summary>
    ///     Event triggered when all snackbars should be hidden.
    /// </summary>
    public event EventHandler? OnHideAll;

    /// <summary>
    ///     Shows a snackbar notification with the specified message and options.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="type">The type of the snackbar.</param>
    /// <param name="theme">The theme of the snackbar.</param>
    /// <param name="duration">The duration to display the snackbar in milliseconds.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ShowAsync(string message, SnackbarType type = SnackbarType.Information,
        ThemeType theme = ThemeType.DarkMode, int duration = 5000)
    {
        var options = new SnackbarNotificationOptions
        {
            Message = message, Type = type, Theme = theme, Duration = duration
        };
        return ShowAsync(options);
    }

    /// <summary>
    ///     Shows a snackbar notification with the specified options.
    /// </summary>
    /// <param name="options">The options for the snackbar notification.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ShowAsync(SnackbarNotificationOptions options)
    {
        try
        {
            OnShow?.Invoke(this, new SnackbarNotificationEventArgs(options));
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(new SnackbarException("Error showing snackbar", ex));
        }
    }

    /// <summary>
    ///     Hides all snackbar notifications.
    /// </summary>
    public void HideAll()
    {
        OnHideAll?.Invoke(this, EventArgs.Empty);
    }
}
