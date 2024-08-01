#region

#endregion

namespace DropBear.Blazor.Arguments.Events;

public sealed class ModalEventArgs<TContext> : EventArgs where TContext : class
{
    public ModalEventArgs(string modalId, string title, bool isVisible, TContext context)
    {
        ModalId = modalId;
        Title = title;

        IsVisible = isVisible;
        Context = context;
    }

    public string ModalId { get; }
    public string Title { get; }

    public bool IsVisible { get; }
    public TContext Context { get; }
}
