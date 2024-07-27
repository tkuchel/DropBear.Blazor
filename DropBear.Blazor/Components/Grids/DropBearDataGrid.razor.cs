#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Grids;

public class DropbearDataGridBase<TItem> : ComponentBase
{
    private DataGridColumn<TItem> _currentSortColumn = new();
    private SortDirection _currentSortDirection = SortDirection.Ascending;

    private List<TItem>? _selectedItems;
    [Parameter] public IEnumerable<TItem> Items { get; set; } = new List<TItem>();
    [Parameter] public List<DataGridColumn<TItem>> Columns { get; set; } = [];
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

    protected bool SelectAll
    {
        get => SelectedItems.Count == Items.Count();
        set
        {
            SelectedItems = value ? [..Items] : [];
            OnSelectionChanged.InvokeAsync(SelectedItems);
        }
    }

    protected List<TItem> SelectedItems
    {
        get => _selectedItems ??= [];
        private set => _selectedItems = value;
    }

    private IEnumerable<TItem> FilteredItems { get; set; } = new List<TItem>();
    protected IEnumerable<TItem> DisplayedItems { get; private set; } = new List<TItem>();
    protected List<int> ItemsPerPageOptions { get; set; } = [10, 25, 50, 100];
    protected string ThemeClass => Theme == ThemeType.DarkMode ? "dark-theme" : "light-theme";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        // Ensure initial state is set
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
                column.PropertySelector != null &&
                column.PropertySelector.Compile()(item).ToString()
                    ?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true
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
            // If clicking the same column, toggle the sort direction
            _currentSortDirection = _currentSortDirection == SortDirection.Ascending
                ? SortDirection.Descending
                : SortDirection.Ascending;
        }
        else
        {
            // If clicking a new column, default to ascending
            _currentSortColumn = column;
            _currentSortDirection = SortDirection.Ascending;
        }

        if (column.CustomSort != null)
        {
            FilteredItems = column.CustomSort(FilteredItems, _currentSortDirection == SortDirection.Ascending);
        }
        else if (column.PropertySelector != null)
        {
            FilteredItems = _currentSortDirection == SortDirection.Ascending
                ? FilteredItems.OrderBy(column.PropertySelector.Compile())
                : FilteredItems.OrderByDescending(column.PropertySelector.Compile());
        }

        UpdateDisplayedItems();
    }

    // You might want to add a method to get the current sort direction for a column
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
        switch (isSelected)
        {
            case true when !SelectedItems.Contains(item):
                SelectedItems.Add(item);
                break;
            case false when SelectedItems.Contains(item):
                SelectedItems.Remove(item);
                break;
        }

        OnSelectionChanged.InvokeAsync(SelectedItems);
        StateHasChanged();
    }

    protected void ToggleSelectAll(bool selectAll)
    {
        if (selectAll)
        {
            SelectedItems = [..Items];
        }
        else
        {
            SelectedItems.Clear();
        }

        OnSelectionChanged.InvokeAsync(SelectedItems);
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
        if (column.PropertySelector == null)
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
