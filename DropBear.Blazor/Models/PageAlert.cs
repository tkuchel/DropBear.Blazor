#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents an alert displayed on a page.
/// </summary>
public sealed class PageAlert
{
    /// <summary>
    ///     Gets or sets the unique identifier for the alert.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Gets or sets the title of the alert.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the message of the alert.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the type of the alert.
    /// </summary>
    public AlertType Type { get; set; } = AlertType.Info;

    /// <summary>
    ///     Gets or sets the theme of the alert.
    /// </summary>
    public ThemeType Theme { get; set; } = ThemeType.DarkMode;

    /// <summary>
    ///     Gets or sets the creation date and time of the alert.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     Gets or sets a value indicating whether the alert is dismissible.
    /// </summary>
    public bool IsDismissible { get; set; } = true;
}
