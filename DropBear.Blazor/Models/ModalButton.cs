#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

public sealed class ModalButton
{
    public string Text { get; init; } = "Close";
    public ButtonColor Type { get; init; } = ButtonColor.Primary;
    public string Icon { get; init; } = "fas fa-times";
    public EventCallback OnClick { get; init; } = EventCallback.Empty;
}
