#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

/// <summary>
///     Interface for a service that manages page alerts.
/// </summary>
public interface IPageAlertService
{
    /// <summary>
    ///     Gets the list of alerts.
    /// </summary>
    IReadOnlyList<PageAlert> Alerts { get; }

    /// <summary>
    ///     Event triggered when the alerts list changes.
    /// </summary>
    event EventHandler<EventArgs> OnChange;

    /// <summary>
    ///     Adds an alert to the list.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message of the alert.</param>
    /// <param name="type">The type of the alert.</param>
    /// <param name="isDismissible">Is the alert dismissible</param>
    /// <param name="theme">The theme of the alert.</param>
    /// <param name="durationMs">The duration in milliseconds for the alert to be displayed.</param>
    void AddAlert(string title, string message, AlertType type, bool isDismissible,
        ThemeType theme = ThemeType.DarkMode,
        int? durationMs = 5000);

    /// <summary>
    ///     Adds an alert to the list async.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message of the alert.</param>
    /// <param name="type">The type of the alert.</param>
    /// <param name="isDismissible">Is the alert dismissible</param>
    /// <param name="theme">The theme of the alert.</param>
    /// <param name="durationMs">The duration in milliseconds for the alert to be displayed.</param>
    Task AddAlertAsync(string title, string message, AlertType type, bool isDismissible, ThemeType theme,
        int? durationMs = 5000);

    /// <summary>
    ///     Removes an alert by its ID.
    /// </summary>
    /// <param name="id">The ID of the alert to remove.</param>
    void RemoveAlert(Guid id);

    /// <summary>
    ///     Removes an alert by its ID async.
    /// </summary>
    /// <param name="id">The ID of the alert to remove.</param>
    Task RemoveAlertAsync(Guid id);

    /// <summary>
    ///     Clears all alerts.
    /// </summary>
    void ClearAlerts();
}
