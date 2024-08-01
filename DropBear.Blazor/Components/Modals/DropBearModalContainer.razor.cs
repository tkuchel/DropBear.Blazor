#region

using DropBear.Blazor.Components.Bases;

#endregion

namespace DropBear.Blazor.Components.Modals;

public partial class DropBearModalContainer : DropBearComponentBase
{
    public void Dispose()
    {
        ModalService.OnChange -= HandleModalServiceChange;
    }

    protected override void OnInitialized()
    {
        ModalService.OnChange += HandleModalServiceChange;
    }

    private void HandleModalServiceChange(object? sender, EventArgs e)
    {
        _ = InvokeAsync(StateHasChanged);
    }
}
