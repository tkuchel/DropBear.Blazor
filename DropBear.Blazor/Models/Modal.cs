#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

public sealed class Modal
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Title { get; init; } = "Modal Title";
    public RenderFragment? BodyContent { get; init; }
    public IReadOnlyCollection<ModalButton> Buttons { get; init; } = Array.Empty<ModalButton>();
    public ThemeType Theme { get; init; } = ThemeType.LightMode;
    public bool CloseOnBackdropClick { get; init; } = true;
    public bool IsVisible { get; set; }
}
