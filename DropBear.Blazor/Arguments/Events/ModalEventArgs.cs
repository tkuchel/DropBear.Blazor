namespace DropBear.Blazor.Arguments.Events;

public class ModalEventArgs : EventArgs
{
    public ModalEventArgs(string modalId)
    {
        ModalId = modalId;
    }

    public string ModalId { get; }
}
