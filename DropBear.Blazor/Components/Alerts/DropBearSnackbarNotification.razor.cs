#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
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
    private bool _isInitialized;
    private bool _shouldRender;

    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public SnackbarType Type { get; set; } = SnackbarType.Information;
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public int Duration { get; set; } = 5000;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public string ActionText { get; set; } = "Dismiss";
    [Parameter] public EventCallback OnAction { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    private bool IsVisible { get; set; }
    [Parameter] public string SnackbarId { get; set; } = Guid.NewGuid().ToString("N");

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

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            await JsRuntime.InvokeVoidAsync("console.log", "Snackbar component rendered");
            await JsRuntime.InvokeVoidAsync("console.log", $"Snackbar ID: {SnackbarId}");
            await ShowAsync();
        }
    }

    public async Task ShowAsync()
    {
        IsVisible = true;
        _shouldRender = true;
        StateHasChanged();

        await Task.Yield(); // Ensure the component has rendered

        Console.WriteLine($"Showing snackbar with ID {SnackbarId} and duration {Duration}ms");
        await JsRuntime.InvokeVoidAsync("DropBearSnackbar.startProgress", SnackbarId, Duration);
    }

    public async Task DismissAsync()
    {
        if (!IsVisible)
        {
            return;
        }

        Console.WriteLine($"Dismissing snackbar with ID {SnackbarId}");
        await HideSnackbarAsync();

        IsVisible = false;
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
            await Console.Error.WriteLineAsync($"Error disposing snackbar: {ex.Message}");
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
            await Console.Error.WriteLineAsync($"Error hiding snackbar: {ex.Message}");
        }
    }
}
