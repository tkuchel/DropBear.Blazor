#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface IModalService
{
    event EventHandler<ModalEventArgs>? OnShow;
    event EventHandler<ModalEventArgs>? OnClose;
    event EventHandler? OnChange;

    void AddModal(Modal modal);
    void RemoveModal(string modalId);
    void Show(string modalId);
    void Close(string modalId);
    bool IsModalVisible(string modalId);
    IEnumerable<Modal> GetAllModals();
}
