#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Configuration model for a button.
/// </summary>
public sealed class ButtonConfig
{
    public ButtonConfig() { }

    public ButtonConfig(string id, string text, ButtonColor type, string icon)
    {
        Id = id;
        Text = text;
        Type = type;
        Icon = icon;
    }

    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public ButtonColor Type { get; set; } = ButtonColor.Default;
    public string Icon { get; set; } = string.Empty;
}
