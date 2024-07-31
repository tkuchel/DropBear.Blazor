namespace DropBear.Blazor.Interfaces;

/// <summary>
///     Interface for a service that manages modals.
/// </summary>
public interface IModalService
{
    /// <summary>
    ///     Event triggered when a modal is shown.
    /// </summary>
    event Action OnShow;

    /// <summary>
    ///     Event triggered when a modal is closed.
    /// </summary>
    event Action OnClose;

    /// <summary>
    ///     Shows a modal.
    /// </summary>
    void Show();

    /// <summary>
    ///     Closes a modal.
    /// </summary>
    void Close();
}
