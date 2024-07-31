#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Alerts;

/// <summary>
///     A container component for displaying page alerts.
/// </summary>
public partial class DropBearPageAlertContainer : ComponentBase, IDisposable
{
    /// <summary>
    ///     Disposes of the alert service subscription.
    /// </summary>
    public void Dispose()
    {
        AlertService.OnChange -= StateHasChanged;
    }

    /// <summary>
    ///     Subscribes to the alert service on initialization.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        AlertService.OnChange += StateHasChanged;
    }
}
