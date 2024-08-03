namespace DropBear.Blazor.Exceptions;

/// <summary>
///     Custom exception for snackbar-related errors.
/// </summary>
public sealed class SnackbarException : Exception
{
    public SnackbarException(string message) : base(message) { }
    public SnackbarException(string message, Exception innerException) : base(message, innerException) { }

    public SnackbarException()
    {
    }
}
