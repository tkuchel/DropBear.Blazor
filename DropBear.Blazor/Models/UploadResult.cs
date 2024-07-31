#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents the result of an upload operation.
/// </summary>
public sealed class UploadResult
{
    /// <summary>
    ///     Gets or sets the status of the upload.
    /// </summary>
    public UploadStatus Status { get; set; } = UploadStatus.Ready;

    /// <summary>
    ///     Gets or sets the message associated with the upload result.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
