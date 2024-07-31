#region

using System.Collections.ObjectModel;
using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Grids;

/// <summary>
///     A Blazor component for rendering a data grid with sorting, searching, and pagination capabilities.
/// </summary>
/// <typeparam name="TItem">The type of the data items.</typeparam>
public class DropbearDataGrid<TItem> : DropBearComponentBase
{
    private DataGridColumn<TItem> _currentSortColumn = new();
    private SortDirection _currentSortDirection = SortDirection.Ascending;

    private Collection<TItem>? _selectedItems;
    [Parameter] public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

    [Parameter]
    public IReadOnlyCollection<DataGridColumn<TItem>> Columns { get; set; } = Array.Empty<DataGridColumn<TItem>>();

    [Parameter] public string Title { get; set; } = "Data Grid";
    [Parameter] public bool EnableSearch { get; set; } = true;
    [Parameter] public bool EnablePagination { get; set; } = true;
    [Parameter] public int ItemsPerPage { get; set; } = 10;
    [Parameter] public bool EnableMultiSelect { get; set; }
    [Parameter] public bool AllowAdd { get; set; } = true;
    [Parameter] public bool AllowEdit { get; set; } = true;
    [Parameter] public bool AllowDelete { get; set; } = true;
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.LightMode;
    [Parameter] public EventCallback<TItem> OnAddItem { get; set; }
    [Parameter] public EventCallback<TItem> OnEditItem { get; set; }
    [Parameter] public EventCallback<TItem> OnDeleteItem { get; set; }
    [Parameter] public EventCallback<List<TItem>> OnSelectionChanged { get; set; }

    protected string SearchTerm { get; private set; } = string.Empty;
    protected int CurrentPage { get; private set; } = 1;
    protected int TotalPages => (int)Math.Ceiling(FilteredItems.Count() / (double)ItemsPerPage);

    private bool SelectAll
    {
        get => SelectedItems.Count == Items.Take(SelectedItems.Count + 1).Count();
        set
        {
            SelectedItems.Clear();
            if (value)
            {
                foreach (var item in Items)
                {
                    SelectedItems.Add(item);
                }
            }

            _ = OnSelectionChanged.InvokeAsync(SelectedItems.ToList());
        }
    }


    protected Collection<TItem> SelectedItems
    {
        get => _selectedItems ??= new Collection<TItem>();
        private set => _selectedItems = value;
    }

    private IEnumerable<TItem> FilteredItems { get; set; } = new List<TItem>();
    protected IEnumerable<TItem> DisplayedItems { get; private set; } = new List<TItem>();

    protected IReadOnlyCollection<int> ItemsPerPageOptions { get; set; } =
        new ReadOnlyCollection<int>(new List<int> { 10, 25, 50, 100 });


    protected string ThemeClass => Theme is ThemeType.DarkMode ? "dark-theme" : "light-theme";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _selectedItems ??= [];
        FilteredItems = Items;
        UpdateDisplayedItems();
    }

    protected void OnSearchInput(ChangeEventArgs e)
    {
        SearchTerm = e.Value?.ToString() ?? string.Empty;
        PerformSearch();
    }

    protected void PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            FilteredItems = Items;
        }
        else
        {
            FilteredItems = Items.Where(item => Columns.Any(column =>
                column.PropertySelector?.Compile()(item)?.ToString()
                    ?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) is true
            ));
        }

        CurrentPage = 1;
        UpdateDisplayedItems();
        StateHasChanged();
    }

    protected void SortBy(DataGridColumn<TItem> column)
    {
        if (!column.Sortable)
        {
            return;
        }

        if (_currentSortColumn == column)
        {
            _currentSortDirection = _currentSortDirection == SortDirection.Ascending
                ? SortDirection.Descending
                : SortDirection.Ascending;
        }
        else
        {
            _currentSortColumn = column;
            _currentSortDirection = SortDirection.Ascending;
        }

        if (column.CustomSort is not null)
        {
            FilteredItems = column.CustomSort(FilteredItems, _currentSortDirection is SortDirection.Ascending);
        }
        else if (column.PropertySelector is not null)
        {
            FilteredItems = _currentSortDirection == SortDirection.Ascending
                ? FilteredItems.OrderBy(column.PropertySelector.Compile())
                : FilteredItems.OrderByDescending(column.PropertySelector.Compile());
        }

        UpdateDisplayedItems();
    }

    protected SortDirection? GetSortDirection(DataGridColumn<TItem> column)
    {
        return _currentSortColumn == column ? _currentSortDirection : null;
    }

    private void UpdateDisplayedItems()
    {
        DisplayedItems = FilteredItems
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage);
    }

    protected void PreviousPage()
    {
        if (CurrentPage <= 1)
        {
            return;
        }

        CurrentPage--;
        UpdateDisplayedItems();
    }

    protected void NextPage()
    {
        if (CurrentPage >= TotalPages)
        {
            return;
        }

        CurrentPage++;
        UpdateDisplayedItems();
    }

    protected void ToggleSelection(TItem item, bool isSelected)
    {
        if (isSelected)
        {
            if (!SelectedItems.Contains(item))
            {
                SelectedItems.Add(item);
            }
        }
        else
        {
            SelectedItems.Remove(item);
        }

        _ = OnSelectionChanged.InvokeAsync(SelectedItems.ToList());
        StateHasChanged();
    }

    protected void ToggleSelectAll(bool selectAll)
    {
        SelectedItems.Clear();
        if (selectAll)
        {
            foreach (var item in Items)
            {
                SelectedItems.Add(item);
            }
        }

        _ = OnSelectionChanged.InvokeAsync(SelectedItems.ToList());
        StateHasChanged();
    }

    protected async Task AddItem()
    {
        await OnAddItem.InvokeAsync(default);
    }

    protected async Task EditItem(TItem item)
    {
        await OnEditItem.InvokeAsync(item);
    }

    protected async Task DeleteItem(TItem item)
    {
        await OnDeleteItem.InvokeAsync(item);
    }

    protected string GetFormattedValue(TItem item, DataGridColumn<TItem> column)
    {
        if (column.PropertySelector is null)
        {
            return string.Empty;
        }

        var value = column.PropertySelector.Compile()(item);
        if (string.IsNullOrEmpty(column.Format))
        {
            return value?.ToString() ?? string.Empty;
        }

        if (value is IFormattable formattable)
        {
            return formattable.ToString(column.Format, null);
        }

        return value?.ToString() ?? string.Empty;
    }
}
