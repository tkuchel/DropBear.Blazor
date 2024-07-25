﻿#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

public class ButtonConfig
{
    public ButtonConfig()
    {

    }
    public ButtonConfig(string id, string text, ButtonType type, string icon)
    {
        Id = id;
        Text = text;
        Type = type;
        Icon = icon;
    }

    public string Id { get; set; }
    public string Text { get; set; }
    public ButtonType Type { get; set; }
    public string Icon { get; set; }
}
