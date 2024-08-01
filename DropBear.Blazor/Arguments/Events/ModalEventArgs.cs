#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Arguments.Events;

public sealed class ModalEventArgs : EventArgs
{
    public ModalEventArgs(string modalId, string title, ThemeType theme, bool isVisible)
    {
        ModalId = modalId;
        Title = title;
        Theme = theme;
        IsVisible = isVisible;
    }

    /// <summary>
    ///     Gets the unique identifier of the modal.
    /// </summary>
    public string ModalId { get; }

    /// <summary>
    ///     Gets the title of the modal.
    /// </summary>
    public string Title { get; }

    /// <summary>
    ///     Gets the theme type of the modal.
    /// </summary>
    public ThemeType Theme { get; }

    /// <summary>
    ///     Gets a value indicating whether the modal is currently visible.
    /// </summary>
    public bool IsVisible { get; }
}
