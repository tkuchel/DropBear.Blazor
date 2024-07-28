#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

public sealed class PageAlertService : IPageAlertService
{
    private readonly List<PageAlert> _alerts = [];
    public IReadOnlyList<PageAlert> Alerts => _alerts.AsReadOnly();

    public event Action? OnChange; // This event is used to notify the UI that the alerts have changed

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

    public void RemoveAlert(Guid id)
    {
        // Check if the alert with the given id exists and is dismissible
        var alert = _alerts.FirstOrDefault(a => a.Id == id);
        if (alert is not { IsDismissible: true })
        {
            return;
        }

        // Remove the alert with the given id
        _alerts.RemoveAll(a => a.Id == id);
        OnChange?.Invoke();
    }

    public void ClearAlerts()
    {
        _alerts.Clear();
        OnChange?.Invoke();
    }

    private async Task RemoveAlertAfterDelay(Guid id, int delayMs)
    {
        await Task.Delay(delayMs);
        RemoveAlert(id);
    }
}
