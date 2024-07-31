#region

using DropBear.Blazor.Interfaces;

#endregion

namespace DropBear.Blazor.Services;

/// <summary>
///     Service to manage the display and closure of modals.
/// </summary>
public class ModalService : IModalService
{
    public event EventHandler? OnShow;
    public event EventHandler? OnClose;

    /// <summary>
    ///     Triggers the OnShow event to display a modal.
    /// </summary>
    public void Show()
    {
        OnShow?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     Triggers the OnClose event to close a modal.
    /// </summary>
    public void Close()
    {
        OnClose?.Invoke(this, EventArgs.Empty);
    }
}
