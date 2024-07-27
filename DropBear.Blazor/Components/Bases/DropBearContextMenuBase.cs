#region

using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Bases;

public class DropBearContextMenuBase : ComponentBase, IAsyncDisposable
{
    private const string DropBearContextMenuJavaScript = @"
        window.dropBearContextMenu = {
            initialize: function (element, dotNetReference) {
                element.addEventListener('contextmenu', (e) => {
                    e.preventDefault();
                    this.show(e.clientX, e.clientY, dotNetReference);
                });

                document.addEventListener('click', () => {
                    dotNetReference.invokeMethodAsync('Hide');
                });
            },

            show: function (x, y, dotNetReference) {
                dotNetReference.invokeMethodAsync('Show', x, y);
            },

            dispose: function (element) {
                // Remove event listeners if necessary
            }
        };";

    private bool _jsInitialized;

    private DotNetObjectReference<DropBearContextMenuBase>? _objectReference;
    protected bool IsVisible;
    protected int Left;
    protected int Top;

    protected ElementReference TriggerElement;
    [Inject] protected IJSRuntime? JsRuntime { get; set; }
    [Inject] protected IDynamicContextMenuService? DynamicContextMenuService { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<ContextMenuItem> MenuItems { get; set; } = [];
    [Parameter] public EventCallback<ContextMenuItem> OnItemClicked { get; set; }
    [Parameter] public string MenuType { get; set; } = string.Empty;
    [Parameter] public object Context { get; set; } = new();
    [Parameter] public bool UseDynamicService { get; set; }

    protected List<ContextMenuItem>? ActiveMenuItems { get; private set; }

    public async ValueTask DisposeAsync()
    {
        if (_jsInitialized)
        {
            try
            {
                if (JsRuntime != null)
                {
                    await JsRuntime.InvokeAsync<object>("dropBearContextMenu.dispose", TriggerElement);
                }

                GC.SuppressFinalize(this);
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
        if (UseDynamicService && DynamicContextMenuService != null)
        {
            ActiveMenuItems = await DynamicContextMenuService.GetMenuItemsAsync(Context, MenuType);
        }
        else
        {
            ActiveMenuItems = MenuItems ?? [];
        }
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
            if (JsRuntime != null)
            {
                await JsRuntime.InvokeAsync<object>("eval", DropBearContextMenuJavaScript);
                await JsRuntime.InvokeAsync<object>("dropBearContextMenu.initialize", TriggerElement, _objectReference);
            }

            _jsInitialized = true;
        }
        catch (JSException)
        {
            // JavaScript interop is not available (e.g., prerendering)
            _jsInitialized = false;
        }
    }

    [JSInvokable]
    public void Show(int left, int top)
    {
        Left = left;
        Top = top;
        IsVisible = true;
        StateHasChanged();
    }

    [JSInvokable]
    public void Hide()
    {
        IsVisible = false;
        StateHasChanged();
    }

    protected async Task OnContextMenu(MouseEventArgs e)
    {
        if (_jsInitialized)
        {
            if (JsRuntime != null)
            {
                await JsRuntime.InvokeAsync<object>("dropBearContextMenu.show", e.ClientX, e.ClientY, _objectReference);
            }
        }
    }

    protected async Task OnItemClick(ContextMenuItem item)
    {
        await OnItemClicked.InvokeAsync(item);
        Hide();
    }
}
