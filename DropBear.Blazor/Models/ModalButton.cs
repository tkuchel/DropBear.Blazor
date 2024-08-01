#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents a button in a modal dialog.
/// </summary>
public sealed class ModalButton<TContext> where TContext : class
{
    /// <summary>
    ///     Gets or initializes the text displayed on the button.
    /// </summary>
    public string Text { get; init; } = "Close";

    /// <summary>
    ///     Gets or initializes the color type of the button.
    /// </summary>
    public ButtonColor Color { get; init; } = ButtonColor.Primary;

    /// <summary>
    ///     Gets or initializes the icon displayed on the button.
    /// </summary>
    public string Icon { get; init; } = "fas fa-times";

    /// <summary>
    ///     Gets or initializes the event callback invoked when the button is clicked.
    /// </summary>
    public EventCallback<ModalEventArgs<TContext>> OnClick { get; init; }
}
