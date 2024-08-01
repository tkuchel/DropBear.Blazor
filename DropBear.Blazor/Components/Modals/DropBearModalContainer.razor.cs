#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Interfaces;
using Microsoft.AspNetCore.Components;

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

    private static RenderFragment RenderModal(IModal modal)
    {
        return builder =>
        {
            var modalType = typeof(DropBearModal<>).MakeGenericType(modal.Context.GetType());
            builder.OpenComponent(0, modalType);
            builder.AddAttribute(1, "Id", modal.Id);
            builder.AddAttribute(2, "Title", modal.Title);
            builder.AddAttribute(3, "Theme", ((dynamic)modal).Theme);
            builder.AddAttribute(4, "CloseOnBackdropClick", ((dynamic)modal).CloseOnBackdropClick);
            builder.AddAttribute(5, "Context", modal.Context);
            builder.AddAttribute(6, "BodyContent", new RenderFragment<object>(context =>
                bodyBuilder =>
                {
                    ((dynamic)modal).BodyContent(bodyBuilder, context);
                }));
            builder.AddAttribute(7, "Buttons", ((dynamic)modal).Buttons);
            builder.CloseComponent();
        };
    }
}
