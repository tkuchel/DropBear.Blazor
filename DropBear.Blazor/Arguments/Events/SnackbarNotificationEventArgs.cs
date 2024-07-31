#region

using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Arguments.Events;

/// <summary>
///     Event arguments for the snackbar notification event.
/// </summary>
public class SnackbarNotificationEventArgs : EventArgs
{
    public SnackbarNotificationEventArgs(SnackbarNotificationOptions options)
    {
        Options = options;
    }

    public SnackbarNotificationOptions Options { get; }
}
