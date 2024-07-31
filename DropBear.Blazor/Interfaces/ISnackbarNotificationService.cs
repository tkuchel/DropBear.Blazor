#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface ISnackbarNotificationService
{
    /// <summary>
    ///     Event triggered when a new snackbar notification should be shown.
    /// </summary>
    event EventHandler<SnackbarNotificationEventArgs> OnShow;

    /// <summary>
    ///     Event triggered when all snackbar notifications should be hidden.
    /// </summary>
    event EventHandler OnHideAll;

    /// <summary>
    ///     Shows a snackbar notification with the specified message and options.
    /// </summary>
    /// <param name="message">The message to display in the snackbar.</param>
    /// <param name="type">The type of the snackbar notification.</param>
    /// <param name="theme">The theme to use for the snackbar.</param>
    /// <param name="duration">The duration in milliseconds for which the snackbar should be visible.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowAsync(string message, SnackbarType type = SnackbarType.Information,
        ThemeType theme = ThemeType.DarkMode, int duration = 5000);

    /// <summary>
    ///     Shows a snackbar notification with the specified options.
    /// </summary>
    /// <param name="options">The options for the snackbar notification.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ShowAsync(SnackbarNotificationOptions options);

    /// <summary>
    ///     Hides all currently visible snackbar notifications.
    /// </summary>
    void HideAll();
}
