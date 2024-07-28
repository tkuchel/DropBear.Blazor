#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

public sealed class PageAlert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public AlertType Type { get; set; } = AlertType.Info;
    public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDismissible { get; set; } = true;
}
