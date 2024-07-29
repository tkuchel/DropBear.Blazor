#region

using DropBear.Blazor.Interfaces;

#endregion

namespace DropBear.Blazor.Services;

public class ModalService : IModalService
{
    public event Action OnShow;
    public event Action OnClose;

    public void Show()
    {
        OnShow?.Invoke();
    }

    public void Close()
    {
        OnClose?.Invoke();
    }
}
