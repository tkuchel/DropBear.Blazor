#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Menus;

public sealed partial class DropBearContextMenu : DropBearComponentBase, IAsyncDisposable
{
    private readonly string _contextMenuId = $"context-menu-{Guid.NewGuid()}";
    private bool _isVisible;
    private bool _jsInitialized;
    private int _left;
    private DotNetObjectReference<DropBearContextMenu>? _objectReference;
    private int _top;

    private ElementReference _triggerElement;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IDynamicContextMenuService? DynamicContextMenuService { get; set; }
    [Inject] private ILogger<DropBearContextMenu> Logger { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public IReadOnlyCollection<ContextMenuItem> MenuItems { get; set; } = Array.Empty<ContextMenuItem>();
    [Parameter] public EventCallback<ContextMenuItem> OnItemClicked { get; set; }
    [Parameter] public string MenuType { get; set; } = string.Empty;
    [Parameter] public object Context { get; set; } = new();
    [Parameter] public bool UseDynamicService { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_jsInitialized)
        {
            try
            {
                await JsRuntime.InvokeVoidAsync("DropBearContextMenu.dispose", _contextMenuId);
            }
            catch (JSException ex)
            {
                Logger.LogError(ex, "Failed to dispose context menu");
            }
        }

        _objectReference?.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objectReference = DotNetObjectReference.Create(this);
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("DropBearContextMenu.initialize", _contextMenuId, _objectReference);
            _jsInitialized = true;
            Logger.LogInformation("ContextMenu initialized with ID: {ContextMenuId}", _contextMenuId);
        }
        catch (JSException ex)
        {
            Logger.LogError(ex, "Failed to initialize JavaScript for ContextMenu");
            _jsInitialized = false;
        }
    }

    [JSInvokable]
    public void Show(int left, int top)
    {
        _left = left;
        _top = top;
        _isVisible = true;
        Logger.LogDebug("Show method invoked: Left={Left}, Top={Top}", left, top);
        StateHasChanged();
    }

    [JSInvokable]
    public void Hide()
    {
        _isVisible = false;
        Logger.LogDebug("Hide method invoked");
        StateHasChanged();
    }

    private async Task OnContextMenu(MouseEventArgs e)
    {
        if (_jsInitialized)
        {
            try
            {
                await JsRuntime.InvokeVoidAsync("DropBearContextMenu.show", _contextMenuId, e.ClientX, e.ClientY);
            }
            catch (JSException ex)
            {
                Logger.LogError(ex, "Failed to show context menu");
            }
        }
        else
        {
            Logger.LogWarning("Attempted to show context menu, but JavaScript is not initialized");
        }
    }

    private async Task OnItemClick(ContextMenuItem item)
    {
        await OnItemClicked.InvokeAsync(item);
        Hide();
    }
}
