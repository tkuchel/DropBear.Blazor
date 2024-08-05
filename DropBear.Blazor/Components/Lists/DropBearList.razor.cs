#region

using DropBear.Blazor.Components.Bases;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Lists;

public sealed partial class DropBearList<T> : DropBearComponentBase where T : class
{
    [Parameter] public IReadOnlyCollection<T>? Items { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string HeaderIcon { get; set; } = string.Empty;
    [Parameter] public string HeaderColor { get; set; } = "#95989C"; // Default to a neutral grey color
    [Parameter] public RenderFragment<T> ItemTemplate { get; set; } = null!;

    private bool IsCollapsed { get; set; }

    private void ToggleCollapse()
    {
        IsCollapsed = !IsCollapsed;
    }
}
