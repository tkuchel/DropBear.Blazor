#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Alerts;

/// <summary>
///     A container component for displaying snackbar notifications.
/// </summary>
public sealed partial class DropBearSnackbarNotificationContainer : DropBearComponentBase, IAsyncDisposable
{
    private readonly List<SnackbarInstance> _snackbars = [];

    [Inject] private ISnackbarNotificationService SnackbarService { get; set; } = null!;

    /// <summary>
    ///     Disposes of the snackbar notification container asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        UnsubscribeSnackbarServiceEvents();

        foreach (var snackbar in _snackbars)
        {
            try
            {
                if (snackbar.ComponentRef is not null)
                {
                    await snackbar.ComponentRef.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
                // The circuit is disconnected, so we can't invoke JavaScript.
                // This is fine, as the snackbars will be removed when the page unloads.
            }
        }
    }

    /// <summary>
    ///     Initializes the snackbar notification container.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        SubscribeSnackbarServiceEvents();
    }

    private void SubscribeSnackbarServiceEvents()
    {
        SnackbarService.OnShow += ShowSnackbarAsync;
        SnackbarService.OnHideAll += HideAllSnackbars;
    }

    private void UnsubscribeSnackbarServiceEvents()
    {
        SnackbarService.OnShow -= ShowSnackbarAsync;
        SnackbarService.OnHideAll -= HideAllSnackbars;
    }

    private void ShowSnackbarAsync(object? sender, SnackbarNotificationEventArgs e)
    {
        _ = InvokeAsync(async () =>
        {
            try
            {
                await ShowSnackbarInternalAsync(e.Options);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error showing snackbar: {ex.Message}");
            }
        });
    }

    private async Task ShowSnackbarInternalAsync(SnackbarNotificationOptions options)
    {
        var snackbar = new SnackbarInstance(options);

        _snackbars.Add(snackbar);
        StateHasChanged();

        await Task.Yield(); // Ensure the component is rendered

        if (snackbar.ComponentRef != null)
        {
            await snackbar.ComponentRef.ShowAsync();
        }

        if (snackbar is { IsDismissible: true, Duration: > 0 })
        {
            _ = RemoveSnackbarAfterDurationAsync(snackbar);
        }
    }

    private async Task RemoveSnackbarAfterDurationAsync(SnackbarInstance snackbar)
    {
        await Task.Delay(snackbar.Duration);
        await RemoveSnackbarAsync(snackbar);
    }

    private async Task RemoveSnackbarAsync(SnackbarInstance snackbar)
    {
        if (_snackbars.Remove(snackbar))
        {
            try
            {
                if (snackbar.ComponentRef is not null)
                {
                    await snackbar.ComponentRef.DismissAsync();
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error removing snackbar: {ex.Message}");
            }

            StateHasChanged();
        }
    }

    private void HideAllSnackbars(object? sender, EventArgs e)
    {
        _ = InvokeAsync(async () =>
        {
            try
            {
                await HideAllSnackbarsInternalAsync();
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error hiding all snackbars: {ex.Message}");
            }
        });
    }

    private async Task HideAllSnackbarsInternalAsync()
    {
        var tasks = _snackbars.Select(s => s.ComponentRef?.DismissAsync() ?? Task.CompletedTask);
        await Task.WhenAll(tasks);
        _snackbars.Clear();
        StateHasChanged();
    }

    private async Task OnSnackbarAction(SnackbarInstance snackbar)
    {
        try
        {
            await snackbar.OnAction.Invoke();

            await RemoveSnackbarAsync(snackbar);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error handling snackbar action: {ex.Message}");
        }
    }

    private sealed class SnackbarInstance : SnackbarNotificationOptions
    {
        public SnackbarInstance(SnackbarNotificationOptions options)
        {
            Id = Guid.NewGuid();
            Title = options.Title;
            Message = options.Message;
            Type = options.Type;
            Theme = options.Theme;
            Duration = options.Duration;
            IsDismissible = options.IsDismissible;
            ActionText = options.ActionText;
            OnAction = options.OnAction;
        }

        public Guid Id { get; }
        public DropBearSnackbarNotification? ComponentRef { get; set; }
    }
}
