#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Alerts;

public partial class DropBearPageAlertContainer : ComponentBase
{
    public void Dispose()
    {
        AlertService.OnChange -= StateHasChanged;
    }

    protected override void OnInitialized()
    {
        AlertService.OnChange += StateHasChanged;
    }
}
