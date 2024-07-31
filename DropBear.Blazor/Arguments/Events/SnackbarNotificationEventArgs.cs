#region

using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Arguments.Events;

/// <summary>
///     Event arguments for the snackbar notification event.
/// </summary>
public sealed class SnackbarNotificationEventArgs(SnackbarNotificationOptions options) : EventArgs
{
    public SnackbarNotificationOptions Options { get; } = options;
}
