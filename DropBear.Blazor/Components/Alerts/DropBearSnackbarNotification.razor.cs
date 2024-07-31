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
    private CancellationTokenSource? _dismissCancellationTokenSource;
    private bool _isDismissed;
    private bool _isDisposed;
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

    /// <summary>
    ///     Disposes the snackbar asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        await CancelDismissalAsync();

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
            _dismissCancellationTokenSource = new CancellationTokenSource();

            if (JsRuntime is not null)
            {
                await JsRuntime.InvokeVoidAsync("DropBearSnackbar.startProgress", _dismissCancellationTokenSource.Token,
                    SnackbarId, Duration);
            }


            await WaitForDismissalAsync();
        }
    }

    public async Task DismissAsync()
    {
        if (_isDismissed)
        {
            return;
        }

        _isDismissed = true;
        await CancelDismissalAsync();
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
        await CancelDismissalAsync();
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

    private async Task CancelDismissalAsync()
    {
        try
        {
            await _dismissCancellationTokenSource?.CancelAsync()!;
            _dismissCancellationTokenSource?.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // CancellationTokenSource was already disposed, ignore
        }
    }

    private async Task DisposeSnackbarAsync()
    {
        try
        {
            if (JsRuntime is not null)
            {
                if (_dismissCancellationTokenSource is not null)
                {
                    await JsRuntime.InvokeVoidAsync("DropBearSnackbar.disposeSnackbar",
                        _dismissCancellationTokenSource.Token, SnackbarId);
                }
            }
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
            if (JsRuntime is not null)
            {
                if (_dismissCancellationTokenSource is not null)
                {
                    await JsRuntime.InvokeVoidAsync("DropBearSnackbar.hideSnackbar",
                        _dismissCancellationTokenSource.Token, SnackbarId);
                }
            }
        }
        catch (JSException ex)
        {
            await Console.Error.WriteLineAsync($"Error hiding snackbar: {ex.Message}");
        }
    }

    private async Task WaitForDismissalAsync()
    {
        try
        {
            if (_dismissCancellationTokenSource is not null)
            {
                await Task.Delay(Duration, _dismissCancellationTokenSource.Token);
            }

            await DismissAsync();
        }
        catch (TaskCanceledException)
        {
            // Dismissal was cancelled, do nothing
        }
    }
}
