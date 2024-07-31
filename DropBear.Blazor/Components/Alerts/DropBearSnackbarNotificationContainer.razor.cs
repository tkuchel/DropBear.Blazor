#region

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
    private readonly List<SnackbarInstance> _snackbars = new();

    [Inject] private ISnackbarNotificationService SnackbarService { get; set; } = default!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

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
                await snackbar.ComponentRef.DisposeAsync();
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

    /// <summary>
    ///     Shows a snackbar notification asynchronously.
    /// </summary>
    /// <param name="options">The options for the snackbar notification.</param>
    public async Task ShowSnackbarAsync(SnackbarNotificationOptions options)
    {
        var snackbar = new SnackbarInstance
        {
            Id = Guid.NewGuid(),
            Title = options.Title,
            Message = options.Message,
            Type = options.Type,
            Theme = options.Theme,
            Duration = options.Duration,
            IsDismissible = options.IsDismissible,
            ActionText = options.ActionText,
            OnAction = options.OnAction,
            ComponentRef = new DropBearSnackbarNotification()
        };

        _snackbars.Add(snackbar);
        StateHasChanged();

        await Task.Yield(); // Ensure the component is rendered
        await snackbar.ComponentRef.ShowAsync();

        _ = RemoveSnackbarAfterDuration(snackbar);
    }

    private async Task RemoveSnackbarAfterDuration(SnackbarInstance snackbar)
    {
        if (snackbar.IsDismissible)
        {
            await Task.Delay(snackbar.Duration);
            await RemoveSnackbarAsync(snackbar);
        }
    }

    private async Task RemoveSnackbarAsync(SnackbarInstance snackbar)
    {
        if (_snackbars.Remove(snackbar))
        {
            try
            {
                await snackbar.ComponentRef.DismissAsync();
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Error removing snackbar: {ex.Message}");
            }

            StateHasChanged();
        }
    }

    private async void HideAllSnackbars()
    {
        var tasks = _snackbars.Select(s => s.ComponentRef.DismissAsync());
        await Task.WhenAll(tasks);
        _snackbars.Clear();
        StateHasChanged();
    }

    private sealed class SnackbarInstance : SnackbarNotificationOptions
    {
        public Guid Id { get; init; }
        public DropBearSnackbarNotification ComponentRef { get; set; } = default!;
    }
}
