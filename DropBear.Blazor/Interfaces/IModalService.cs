#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface IModalService<TContext> where TContext : class
{
    event EventHandler<ModalEventArgs<TContext>>? OnShow;

    event EventHandler<ModalEventArgs<TContext>>? OnClose;
    // ... other members

    event EventHandler? OnChange;

    void AddModal<TContext>(Modal<TContext> modal) where TContext : class;
    void RemoveModal(string modalId);
    void Show(string modalId);
    void Close(string modalId);
    bool IsModalVisible(string modalId);
    IEnumerable<IModal> GetAllModals();
}
