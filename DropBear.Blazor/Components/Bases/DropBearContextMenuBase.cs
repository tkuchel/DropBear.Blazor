#region

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
    protected int _left;
    protected int _top;
    protected bool IsVisible;

    private DotNetObjectReference<DropBearContextMenuBase> ObjectReference;

    protected ElementReference TriggerElement;
    [Inject] protected IJSRuntime JSRuntime { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public List<ContextMenuItem> MenuItems { get; set; } = new();
    [Parameter] public EventCallback<ContextMenuItem> OnItemClicked { get; set; }
    [Parameter] public object Context { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_jsInitialized)
        {
            try
            {
                await JSRuntime.InvokeAsync<object>("dropBearContextMenu.dispose", TriggerElement);
            }
            catch (JSException)
            {
                // JavaScript interop is not available
            }
        }

        ObjectReference?.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        try
        {
            await JSRuntime.InvokeAsync<object>("eval", DropBearContextMenuJavaScript);
            await JSRuntime.InvokeAsync<object>("dropBearContextMenu.initialize", TriggerElement, ObjectReference);
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
        _left = left;
        _top = top;
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
            await JSRuntime.InvokeAsync<object>("dropBearContextMenu.show", e.ClientX, e.ClientY, ObjectReference);
        }
    }

    protected async Task OnItemClick(ContextMenuItem item)
    {
        await OnItemClicked.InvokeAsync(item);
        Hide();
    }
}
