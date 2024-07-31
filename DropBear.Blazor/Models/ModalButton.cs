#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents a button in a modal dialog.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ModalButton
{
    /// <summary>
    ///     Gets the text displayed on the button.
    /// </summary>
    public string Text { get; init; } = "Close";

    /// <summary>
    ///     Gets the color type of the button.
    /// </summary>
    public ButtonColor Type { get; init; } = ButtonColor.Primary;

    /// <summary>
    ///     Gets the icon displayed on the button.
    /// </summary>
    public string Icon { get; init; } = "fas fa-times";

    /// <summary>
    ///     Gets the event callback invoked when the button is clicked.
    /// </summary>
    public EventCallback OnClick { get; init; } = EventCallback.Empty;
}
