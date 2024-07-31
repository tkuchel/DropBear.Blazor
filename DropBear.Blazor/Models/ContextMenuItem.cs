#region

using System.Collections.ObjectModel;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents an item in a context menu.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ContextMenuItem
{
    public string Text { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public bool IsSeparator { get; set; }
    public bool IsDanger { get; set; }
    public bool HasSubmenu => Submenu.Count > 0;
    public Collection<ContextMenuItem> Submenu { get; set; } = [];
    public object Data { get; set; } = new();
}
