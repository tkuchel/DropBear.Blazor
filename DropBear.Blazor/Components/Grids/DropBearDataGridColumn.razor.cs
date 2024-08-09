#region

using System.Linq.Expressions;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Grids;

public sealed partial class DropBearDataGridColumn<TItem> : ComponentBase
{
    private bool _isInitialized;

    [CascadingParameter] private DropBearDataGrid<TItem> ParentGrid { get; set; } = default!;

    [Parameter] public string PropertyName { get; set; } = string.Empty;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public Expression<Func<TItem, object>> PropertySelector { get; set; } = default!;
    [Parameter] public bool Sortable { get; set; }
    [Parameter] public bool Filterable { get; set; }
    [Parameter] public string Format { get; set; } = string.Empty;
    [Parameter] public int Width { get; set; }
    [Parameter] public RenderFragment<TItem> Template { get; set; } = default!;
    [Parameter] public Func<IEnumerable<TItem>, bool, IEnumerable<TItem>> CustomSort { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (_isInitialized)
        {
            return;
        }

        if (ParentGrid is null)
        {
            throw new InvalidOperationException(
                $"{nameof(DropBearDataGridColumn<TItem>)} must be used within a {nameof(DropBearDataGrid<TItem>)}");
        }

        var column = new DataGridColumn<TItem>
        {
            PropertyName = PropertyName,
            Title = Title,
            PropertySelector = PropertySelector,
            Sortable = Sortable,
            Filterable = Filterable,
            Format = Format,
            Width = Width,
            Template = Template,
            CustomSort = CustomSort
        };

        ParentGrid.AddColumn(column);
        _isInitialized = true;
    }
}
