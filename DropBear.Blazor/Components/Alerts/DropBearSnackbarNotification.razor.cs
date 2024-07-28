#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Alerts;

public sealed partial class DropBearSnackbarNotification : DropBearComponentBase, IAsyncDisposable
{
    private CancellationTokenSource? _dismissCancellationTokenSource;
    private bool _isDismissed = false;
    private bool _isDisposed = false;
    private bool _isInitialized;
    private bool _shouldRender;
    [Inject] private IJSRuntime? JsRuntime { get; set; } = default!;

    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public SnackbarType Type { get; set; } = SnackbarType.Information;
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public int Duration { get; set; } = 5000;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public string ActionText { get; set; } = "Dismiss";
    [Parameter] public EventCallback OnAction { get; set; }

    private bool IsVisible { get; set; }
    [Parameter] public string SnackbarId { get; set; } = Guid.NewGuid().ToString("N");

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        try
        {
            await _dismissCancellationTokenSource?.CancelAsync();
            _dismissCancellationTokenSource?.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // CancellationTokenSource was already disposed, ignore
        }

        if (!_isDismissed)
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
            await InvokeAsync(StateHasChanged);
        }
    }

    public async Task ShowAsync()
    {
        IsVisible = true;
        _shouldRender = true;
        StateHasChanged();

        if (Duration > 0)
        {
            await Task.Yield(); // Ensure the component has rendered
            if (JsRuntime != null)
            {
                await JsRuntime.InvokeVoidAsync("DropBearSnackbar.startProgress", SnackbarId, Duration);
            }

            _dismissCancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Task.Delay(Duration, _dismissCancellationTokenSource.Token);
                await DismissAsync();
            }
            catch (TaskCanceledException)
            {
                // Dismissal was cancelled, do nothing
            }
        }
    }

    public async Task DismissAsync()
    {
        if (_isDismissed)
        {
            return;
        }

        _isDismissed = true;
        try
        {
            await _dismissCancellationTokenSource?.CancelAsync();
        }
        catch (ObjectDisposedException)
        {
            // CancellationTokenSource was already disposed, ignore
        }

        try
        {
            if (JsRuntime != null)
            {
                await JsRuntime.InvokeVoidAsync("DropBearSnackbar.hideSnackbar", SnackbarId);
            }
        }
        catch (JSException ex)
        {
            await Console.Error.WriteLineAsync($"Error hiding snackbar: {ex.Message}");
        }

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
        try
        {
            await _dismissCancellationTokenSource?.CancelAsync();
        }
        catch (ObjectDisposedException)
        {
            // CancellationTokenSource was already disposed, ignore
        }

        await OnAction.InvokeAsync();
        await DismissAsync();
    }

    private string GetSnackbarClasses()
    {
        return $"snackbar-{GetThemeClass()} snackbar-{Type.ToString().ToLower()}";
    }

    private string GetThemeClass()
    {
        return Theme == ThemeType.DarkMode ? "dark" : "light";
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
}
