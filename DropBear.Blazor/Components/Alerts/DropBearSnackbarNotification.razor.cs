#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Alerts;

/// <summary>
///     A Blazor component for displaying snackbar notifications.
/// </summary>
public sealed partial class DropBearSnackbarNotification : DropBearComponentBase, IAsyncDisposable
{
    private bool _isDismissed;
    private bool _isDisposed;
    private bool _shouldRender;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the title of the snackbar.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the message of the snackbar.
    /// </summary>
    [Parameter]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the type of the snackbar.
    /// </summary>
    [Parameter]
    public SnackbarType Type { get; set; } = SnackbarType.Information;

    /// <summary>
    ///     Gets or sets the theme of the snackbar.
    /// </summary>
    [Parameter]
    public ThemeType Theme { get; set; } = ThemeType.DarkMode;

    /// <summary>
    ///     Gets or sets the duration of the snackbar in milliseconds.
    /// </summary>
    [Parameter]
    public int Duration { get; set; } = 5000;

    /// <summary>
    ///     Gets or sets a value indicating whether the snackbar is dismissible.
    /// </summary>
    [Parameter]
    public bool IsDismissible { get; set; } = true;

    /// <summary>
    ///     Gets or sets the text for the action button.
    /// </summary>
    [Parameter]
    public string ActionText { get; set; } = "Dismiss";

    /// <summary>
    ///     Gets or sets the callback to be invoked when the action button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnAction { get; set; }

    /// <summary>
    ///     Gets or sets additional attributes for the snackbar element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Gets or sets the unique identifier for the snackbar.
    /// </summary>
    [Parameter]
    public string SnackbarId { get; set; } = Guid.NewGuid().ToString("N");

    private bool IsVisible { get; set; }

    /// <summary>
    ///     Disposes of the component and its resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        if (!_isDismissed)
        {
            await DisposeSnackbarAsync();
        }
    }

    /// <summary>
    ///     Determines whether the component should render.
    /// </summary>
    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    /// <summary>
    ///     Performs actions after the component has rendered.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ShowAsync();
        }
    }

    /// <summary>
    ///     Shows the snackbar.
    /// </summary>
    public async Task ShowAsync()
    {
        if (IsVisible)
        {
            return;
        }

        IsVisible = true;
        _shouldRender = true;
        StateHasChanged();

        await Task.Yield();

        try
        {
            await JsRuntime.InvokeVoidAsync("DropBearSnackbar.startProgress", SnackbarId, Duration);
        }
        catch (JSException ex)
        {
            throw new SnackbarException("Error showing snackbar", ex);
        }
    }

    /// <summary>
    ///     Dismisses the snackbar.
    /// </summary>
    public async Task DismissAsync()
    {
        if (!IsVisible)
        {
            return;
        }

        try
        {
            await HideSnackbarAsync();
        }
        catch (JSException ex)
        {
            throw new SnackbarException("Error dismissing snackbar", ex);
        }

        IsVisible = false;
        _isDismissed = true;
        _shouldRender = true;
        StateHasChanged();
    }

    private void Dismiss()
    {
        _ = DismissAsync();
    }

    private async Task OnActionClick()
    {
        await OnAction.InvokeAsync();
        await DismissAsync();
    }

    private string GetSnackbarClasses()
    {
#pragma warning disable CA1308
        return $"snackbar-{GetThemeClass()} snackbar-{Type.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308
    }

    private string GetThemeClass()
    {
        return Theme is ThemeType.DarkMode ? "dark" : "light";
    }

    private string GetIconClass()
    {
        return Type switch
        {
            SnackbarType.Information => "fas fa-info-circle",
            SnackbarType.Success => "fas fa-check-circle",
            SnackbarType.Warning => "fas fa-exclamation-triangle",
            SnackbarType.Error => "fas fa-times-circle",
            _ => "fas fa-info-circle"
        };
    }

    private async Task DisposeSnackbarAsync()
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("DropBearSnackbar.disposeSnackbar", SnackbarId);
        }
        catch (JSException ex)
        {
            throw new SnackbarException("Error disposing snackbar", ex);
        }
    }

    private async Task HideSnackbarAsync()
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("DropBearSnackbar.hideSnackbar", SnackbarId);
        }
        catch (JSException ex)
        {
            throw new SnackbarException("Error hiding snackbar", ex);
        }
    }
}
