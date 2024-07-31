#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents the options for displaying a snackbar notification.
/// </summary>
public class SnackbarNotificationOptions
{
    /// <summary>
    ///     Gets the title of the snackbar notification.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the message of the snackbar notification.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the type of the snackbar notification.
    /// </summary>
    public SnackbarType Type { get; init; } = SnackbarType.Information;

    /// <summary>
    ///     Gets the theme of the snackbar notification.
    /// </summary>
    public ThemeType Theme { get; init; } = ThemeType.DarkMode;

    /// <summary>
    ///     Gets the duration in milliseconds for which the snackbar notification is displayed.
    /// </summary>
    public int Duration { get; init; } = 5000;

    /// <summary>
    ///     Gets a value indicating whether the snackbar notification is dismissible.
    /// </summary>
    public bool IsDismissible { get; init; } = true;

    /// <summary>
    ///     Gets the text of the action button on the snackbar notification.
    /// </summary>
    public string ActionText { get; init; } = "Dismiss";

    /// <summary>
    ///     Gets the action to perform when the action button is clicked.
    /// </summary>
    public Func<Task> OnAction { get; init; } = () => Task.CompletedTask;
}
