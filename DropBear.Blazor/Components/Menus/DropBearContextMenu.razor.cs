#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Menus;

/// <summary>
///     A Blazor component for displaying a context menu.
/// </summary>
public sealed partial class DropBearContextMenu : DropBearComponentBase, IAsyncDisposable
{
    private CancellationTokenSource? _dismissCancellationTokenSource;
    private bool _isVisible;
    private bool _jsInitialized;
    private int _left;
    private DotNetObjectReference<DropBearContextMenu>? _objectReference;
    private int _top;
    private ElementReference? _triggerElement; // The element that will trigger the context menu
    [Inject] private IJSRuntime? JsRuntime { get; set; }
    [Inject] private IDynamicContextMenuService? DynamicContextMenuService { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public IReadOnlyCollection<ContextMenuItem> MenuItems { get; set; } = Array.Empty<ContextMenuItem>();
    [Parameter] public EventCallback<ContextMenuItem> OnItemClicked { get; set; }
    [Parameter] public string MenuType { get; set; } = string.Empty;
    [Parameter] public object Context { get; set; } = new();
    [Parameter] public bool UseDynamicService { get; set; }

    /// <summary>
    ///     Disposes the context menu.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_jsInitialized && JsRuntime is not null)
        {
            try
            {
                if (_dismissCancellationTokenSource is not null)
                {
                    await JsRuntime.InvokeVoidAsync("dropBearContextMenu.dispose",
                        _dismissCancellationTokenSource.Token, _triggerElement);

                    _dismissCancellationTokenSource.Dispose();
                }
            }
            catch (JSException)
            {
                // JavaScript interop is not available
            }
        }

        _objectReference?.Dispose();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (UseDynamicService && DynamicContextMenuService is not null)
        {
            await DynamicContextMenuService.GetMenuItemsAsync(Context, MenuType);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dismissCancellationTokenSource = new CancellationTokenSource();
            _objectReference = DotNetObjectReference.Create(this);
            await InitializeJavaScript();
        }
    }

    /// <summary>
    ///     Initializes the JavaScript interop.
    /// </summary>
    private async Task InitializeJavaScript()
    {
        if (JsRuntime is not null)
        {
            try
            {
                if (_dismissCancellationTokenSource is not null)
                {
                    await JsRuntime.InvokeVoidAsync("dropBearContextMenu.initialize",
                        _dismissCancellationTokenSource.Token, _triggerElement,
                        _objectReference);
                }

                _jsInitialized = true;
            }
            catch (JSException)
            {
                // JavaScript interop is not available (e.g., prerendering)
                _jsInitialized = false;
            }
        }
    }

    [JSInvokable]
    public void Show(int left, int top)
    {
        _left = left;
        _top = top;
        _isVisible = true;
        StateHasChanged();
    }

    [JSInvokable]
    public void Hide()
    {
        _isVisible = false;
        StateHasChanged();
    }

    private async Task OnContextMenu(MouseEventArgs e)
    {
        if (_jsInitialized && JsRuntime is not null)
        {
            if (_dismissCancellationTokenSource is not null)
            {
                await JsRuntime.InvokeVoidAsync("dropBearContextMenu.show", _dismissCancellationTokenSource.Token,
                    e.ClientX, e.ClientY, _objectReference);
            }
        }
    }

    private async Task OnItemClick(ContextMenuItem item)
    {
        await OnItemClicked.InvokeAsync(item);
        Hide();
    }
}
