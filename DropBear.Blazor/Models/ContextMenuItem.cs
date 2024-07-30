namespace DropBear.Blazor.Models;

public sealed class ContextMenuItem
{
    public string Text { get; set; }
    public string IconClass { get; set; }
    public bool IsSeparator { get; set; }
    public bool IsDanger { get; set; }
    public bool HasSubmenu => Submenu?.Count > 0;
    public List<ContextMenuItem> Submenu { get; set; }
    public object Data { get; set; }
}
