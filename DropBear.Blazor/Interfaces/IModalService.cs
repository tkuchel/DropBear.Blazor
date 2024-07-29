namespace DropBear.Blazor.Interfaces;

public interface IModalService
{
    event Action OnShow;
    event Action OnClose;
    void Show();
    void Close();
}
