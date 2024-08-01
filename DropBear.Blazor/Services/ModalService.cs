#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Services;

public class ModalService : IModalService
{
    private readonly Dictionary<string, IModal> _modals = new(StringComparer.Ordinal);

    public event EventHandler<ModalEventArgs<object>>? OnShow;
    public event EventHandler<ModalEventArgs<object>>? OnClose;
    public event EventHandler? OnChange;

    public void AddModal<TContext>(Modal<TContext> modal) where TContext : class
    {
        _modals[modal.Id] = modal;
        OnChange?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveModal(string modalId)
    {
        if (_modals.Remove(modalId))
        {
            OnChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Show(string modalId)
    {
        if (_modals.TryGetValue(modalId, out var modal))
        {
            modal.IsVisible = true;
            OnShow?.Invoke(this, new ModalEventArgs<object>(modal.Id, modal.Title, modal.IsVisible, modal.Context));
            OnChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Close(string modalId)
    {
        if (_modals.TryGetValue(modalId, out var modal))
        {
            modal.IsVisible = false;
            OnClose?.Invoke(this, new ModalEventArgs<object>(modal.Id, modal.Title, modal.IsVisible, modal.Context));
            OnChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsModalVisible(string modalId)
    {
        return _modals.TryGetValue(modalId, out var modal) && modal.IsVisible;
    }

    public IEnumerable<IModal> GetAllModals()
    {
        return _modals.Values;
    }
}
