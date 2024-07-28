#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Alerts;

public sealed partial class DropBearSnackbarNotificationContainer : DropBearComponentBase, IAsyncDisposable
{
    private readonly List<SnackbarInstance> _snackbars = [];


    [Parameter] public RenderFragment? ChildContent { get; set; }

    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }

    protected override void OnInitialized()
    {
        SnackbarService.OnShow += ShowSnackbarAsync;
        SnackbarService.OnHideAll += HideAllSnackbars;
    }

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

        // Defer the call to ShowAsync
        await Task.Yield(); // Ensure component is rendered
        await snackbar.ComponentRef.ShowAsync();

        _ = RemoveSnackbarAfterDuration(snackbar);
    }

    private async Task RemoveSnackbarAfterDuration(SnackbarInstance snackbar)
    {
        if (snackbar.IsDismissible)
        {
            await Task.Delay(snackbar.Duration);
            _snackbars.Remove(snackbar);
            StateHasChanged();
        }
    }

    private void HideAllSnackbars()
    {
        _snackbars.Clear();
        StateHasChanged();
    }

    private sealed class SnackbarInstance : SnackbarNotificationOptions
    {
        public Guid Id { get; init; }
        public DropBearSnackbarNotification ComponentRef { get; set; }
    }
}
