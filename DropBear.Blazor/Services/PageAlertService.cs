#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

/// <summary>
///     Service to manage page alerts.
/// </summary>
public sealed class PageAlertService : IPageAlertService
{
    private readonly List<PageAlert> _alerts = new();
    public IReadOnlyList<PageAlert> Alerts => _alerts.AsReadOnly();

    public event Action? OnChange; // Event to notify the UI that the alerts have changed

    /// <summary>
    ///     Adds an alert to the list.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message of the alert.</param>
    /// <param name="type">The type of the alert.</param>
    /// <param name="theme">The theme of the alert.</param>
    /// <param name="durationMs">The duration in milliseconds for the alert to be displayed.</param>
    public void AddAlert(string title, string message, AlertType type, ThemeType theme = ThemeType.DarkMode,
        int? durationMs = 5000)
    {
        var alert = new PageAlert
        {
            Id = Guid.NewGuid(),
            Title = title,
            Message = message,
            Type = type,
            Theme = theme,
            CreatedAt = DateTime.Now
        };

        _alerts.Add(alert);
        OnChange?.Invoke();

        if (durationMs.HasValue)
        {
            _ = RemoveAlertAfterDelay(alert.Id, durationMs.Value);
        }
    }

    /// <summary>
    ///     Removes an alert by its ID.
    /// </summary>
    /// <param name="id">The ID of the alert to remove.</param>
    public void RemoveAlert(Guid id)
    {
        var alert = _alerts.FirstOrDefault(a => a.Id == id);
        if (alert is not { IsDismissible: true })
        {
            return;
        }

        _alerts.RemoveAll(a => a.Id == id);
        OnChange?.Invoke();
    }

    /// <summary>
    ///     Clears all alerts from the list.
    /// </summary>
    public void ClearAlerts()
    {
        _alerts.Clear();
        OnChange?.Invoke();
    }

    /// <summary>
    ///     Removes an alert after a specified delay.
    /// </summary>
    /// <param name="id">The ID of the alert to remove.</param>
    /// <param name="delayMs">The delay in milliseconds before removing the alert.</param>
    private async Task RemoveAlertAfterDelay(Guid id, int delayMs)
    {
        await Task.Delay(delayMs);
        RemoveAlert(id);
    }
}
