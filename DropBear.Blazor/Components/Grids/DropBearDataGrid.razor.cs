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
public sealed partial class DropBearDataGrid<TItem> : DropBearComponentBase
{
    private readonly List<DataGridColumn<TItem>> _columns = new();
    private DataGridColumn<TItem> _currentSortColumn = new();
    private SortDirection _currentSortDirection = SortDirection.Ascending;

    private Collection<TItem>? _selectedItems;
    [Parameter] public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

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

    [Parameter] public RenderFragment Columns { get; set; } = default!;

    private string SearchTerm { get; set; } = string.Empty;
    private int CurrentPage { get; set; } = 1;
    private int TotalPages => (int)Math.Ceiling(FilteredItems.Count() / (double)ItemsPerPage);

    private Collection<TItem> SelectedItems
    {
        get => _selectedItems ??= [];
        // ReSharper disable once UnusedMember.Local
        set => _selectedItems = value;
    }

    private IEnumerable<TItem> FilteredItems { get; set; } = new List<TItem>();
    private IEnumerable<TItem> DisplayedItems { get; set; } = new List<TItem>();

    private IReadOnlyCollection<int> ItemsPerPageOptions { get; set; } =
        new ReadOnlyCollection<int>(new List<int> { 10, 25, 50, 100 });

    private string ThemeClass => Theme is ThemeType.DarkMode ? "dark-theme" : "light-theme";

    public IReadOnlyCollection<DataGridColumn<TItem>> GetColumns => _columns.AsReadOnly();


    public void AddColumn(DataGridColumn<TItem> column)
    {
        _columns.Add(column);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _selectedItems ??= [];
        FilteredItems = Items;
        UpdateDisplayedItems();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            StateHasChanged();
        }
    }

    private void OnSearchInput(ChangeEventArgs e)
    {
        SearchTerm = e.Value?.ToString() ?? string.Empty;
        PerformSearch();
    }

    private void PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            FilteredItems = Items;
        }
        else
        {
            FilteredItems = Items.Where(item => _columns.Exists(column =>
                column.PropertySelector?.Compile()(item).ToString()
                    ?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) is true
            ));
        }

        CurrentPage = 1;
        UpdateDisplayedItems();
        StateHasChanged();
    }

    private void SortBy(DataGridColumn<TItem> column)
    {
        if (!column.Sortable)
        {
            return;
        }

        if (_currentSortColumn == column)
        {
            _currentSortDirection = _currentSortDirection is SortDirection.Ascending
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
            FilteredItems = _currentSortDirection is SortDirection.Ascending
                ? FilteredItems.OrderBy(column.PropertySelector.Compile())
                : FilteredItems.OrderByDescending(column.PropertySelector.Compile());
        }

        UpdateDisplayedItems();
    }

    private SortDirection? GetSortDirection(DataGridColumn<TItem> column)
    {
        return _currentSortColumn == column ? _currentSortDirection : null;
    }

    private void UpdateDisplayedItems()
    {
        DisplayedItems = FilteredItems
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage);
    }

    private void PreviousPage()
    {
        if (CurrentPage <= 1)
        {
            return;
        }

        CurrentPage--;
        UpdateDisplayedItems();
    }

    private void NextPage()
    {
        if (CurrentPage >= TotalPages)
        {
            return;
        }

        CurrentPage++;
        UpdateDisplayedItems();
    }

    private void ToggleSelection(TItem item, bool isSelected)
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

    private void ToggleSelectAll(bool selectAll)
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

    private async Task AddItem()
    {
        await OnAddItem.InvokeAsync(default);
    }

    private async Task EditItem(TItem item)
    {
        await OnEditItem.InvokeAsync(item);
    }

    private async Task DeleteItem(TItem item)
    {
        await OnDeleteItem.InvokeAsync(item);
    }

    private static string GetFormattedValue(TItem item, DataGridColumn<TItem> column)
    {
        if (column.PropertySelector is null)
        {
            return string.Empty;
        }

        var value = column.PropertySelector.Compile()(item);
        if (string.IsNullOrEmpty(column.Format))
        {
            return value.ToString() ?? string.Empty;
        }

        if (value is IFormattable formattable)
        {
            return formattable.ToString(column.Format, null);
        }

        return value.ToString() ?? string.Empty;
    }
}
