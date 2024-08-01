#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

public class Modal
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = "Modal Title";
    public RenderFragment? BodyContent { get; set; }
    public IReadOnlyCollection<ModalButton> Buttons { get; set; } = Array.Empty<ModalButton>();
    public ThemeType Theme { get; set; } = ThemeType.LightMode;
    public bool CloseOnBackdropClick { get; set; } = true;
    public bool IsVisible { get; set; }
}
