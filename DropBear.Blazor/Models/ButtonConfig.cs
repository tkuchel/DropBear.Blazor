#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

public sealed class ButtonConfig
{
    public ButtonConfig()
    {
    }

    public ButtonConfig(string id, string text, ButtonColor type, string icon)
    {
        Id = id;
        Text = text;
        Type = type;
        Icon = icon;
    }

    public string Id { get; set; }
    public string Text { get; set; }
    public ButtonColor Type { get; set; }
    public string Icon { get; set; }
}
