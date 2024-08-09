#region

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents a column in a data grid.
/// </summary>
/// <typeparam name="TItem">The type of the data items.</typeparam>
public sealed class DataGridColumn<TItem>
{
    // Existing properties
    public string PropertyName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Sortable { get; set; }
    public bool Filterable { get; set; }
    public double Width { get; set; } = 100; // Default width

    // Enhanced property selector using Expression for better type safety and performance
    public Expression<Func<TItem, object>>? PropertySelector { get; set; }

    // Existing template property
    public RenderFragment<TItem>? Template { get; set; }

    // New properties
    public string CssClass { get; set; } = string.Empty;
    public bool Visible { get; set; } = true;
    public string Format { get; set; } = string.Empty;

    // Custom sort function
    public Func<IEnumerable<TItem>, bool, IEnumerable<TItem>>? CustomSort { get; set; }
    public Func<IEnumerable<TItem>, string, IEnumerable<TItem>>? CustomFilter { get; set; }

    // Header template
    public RenderFragment? HeaderTemplate { get; set; }

    // Footer template
    public RenderFragment? FooterTemplate { get; set; }
}
