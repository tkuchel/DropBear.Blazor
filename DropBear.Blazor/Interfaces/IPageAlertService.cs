#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface IPageAlertService
{
    IReadOnlyList<PageAlert> Alerts { get; }
    event Action OnChange;

    void AddAlert(string title, string message, AlertType type, ThemeType theme = ThemeType.DarkMode,
        int? durationMs = 5000);

    void RemoveAlert(Guid id);
    void ClearAlerts();
}
