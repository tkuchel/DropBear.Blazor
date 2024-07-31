#region

using System.Collections.ObjectModel;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents an item in a context menu.
/// </summary>
public sealed class ContextMenuItem
{
    public ContextMenuItem()
    {
        Text = string.Empty;
        IconClass = string.Empty;
        Submenu = new Collection<ContextMenuItem>();
        Data = new object();
    }

    public string Text { get; set; }
    public string IconClass { get; set; }
    public bool IsSeparator { get; set; }
    public bool IsDanger { get; set; }
    public bool HasSubmenu => Submenu.Count > 0;
    public Collection<ContextMenuItem> Submenu { get; set; }
    public object Data { get; set; }
}
