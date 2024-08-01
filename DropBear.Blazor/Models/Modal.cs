#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Interfaces;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

public sealed class Modal<TContext> : IModal where TContext : class
{
    public RenderFragment<TContext>? BodyContent { get; set; }
    public IReadOnlyCollection<ModalButton<TContext>> Buttons { get; set; } = Array.Empty<ModalButton<TContext>>();

    public bool CloseOnBackdropClick { get; set; } = true;
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Title { get; init; } = "Modal Title";
    public ThemeType Theme { get; init; } = ThemeType.LightMode;
    public bool IsVisible { get; set; }
    public Object Context { get; init; } = default!;
}
