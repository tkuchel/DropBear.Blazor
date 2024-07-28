#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Alerts;

public sealed partial class DropBearSnackbarNotification : DropBearComponentBase, IAsyncDisposable
{
    private bool _isInitialized;
    private bool _shouldRender;
    [Inject] private IJSRuntime? JsRuntime { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public SnackbarType Type { get; set; } = SnackbarType.Information;
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public int Duration { get; set; } = 5000;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public string ActionText { get; set; } = "Dismiss";
    [Parameter] public EventCallback OnAction { get; set; }

    private bool IsVisible { get; set; }
    [Parameter] public string? SnackbarId { get; set; }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(SnackbarId))
        {
            SnackbarId = Guid.NewGuid().ToString();
        }
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;
        await InvokeAsync(StateHasChanged); // Ensure StateHasChanged is called after render handle is available
    }

    public async Task ShowAsync()
    {
        IsVisible = true;
        _shouldRender = true;
        StateHasChanged();

        if (Duration > 0)
        {
            var escapedSnackbarId = SnackbarId!;
            if (JsRuntime != null)
            {
                await JsRuntime.InvokeVoidAsync("eval", GetJavaScriptFunctions());
                await JsRuntime.InvokeVoidAsync("startProgress", escapedSnackbarId, Duration);
            }

            await Task.Delay(Duration);
            await DismissAsync();
        }
    }

    public async Task DismissAsync()
    {
        var escapedSnackbarId = SnackbarId!;
        if (JsRuntime != null)
        {
            await JsRuntime.InvokeVoidAsync("hideSnackbar", escapedSnackbarId);
        }

        await Task.Delay(300); // Wait for the hide animation to complete
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

    private string GetJavaScriptFunctions()
    {
        return @"
            function startProgress(snackbarId, duration) {
                const progress = document.querySelector(`#${snackbarId} .snackbar-progress`);
                if (progress) {
                    progress.style.transition = `width ${duration}ms linear`;
                    progress.style.width = '0%';
                } else {
                    console.error('Progress bar not found');
                }
            }

            function showSnackbar(snackbarId, duration) {
                const snackbar = document.getElementById(snackbarId);
                if (snackbar) {
                    snackbar.style.display = 'block';
                    setTimeout(() => {
                        snackbar.style.display = 'none';
                    }, duration);
                } else {
                    console.error('Snackbar not found');}
            }

            function hideSnackbar(snackbarId) {
                const snackbar = document.querySelector(`#${snackbarId}`);
                if (snackbar) {
                    snackbar.style.animation = 'slideOutAndShrink var(--transition-normal) ease-out forwards';
                } else {
                    console.error('Snackbar not found');
                }
            }
        ";
    }
}
