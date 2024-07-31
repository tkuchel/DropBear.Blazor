#region

using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components.Forms;

#endregion

namespace DropBear.Blazor.Models;

/// <summary>
///     Represents a file to be uploaded.
/// </summary>
public sealed class UploadFile
{
    /// <summary>
    ///     Gets or sets the name of the file.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the size of the file in bytes.
    /// </summary>
    public long Size { get; set; } = 0;

    /// <summary>
    ///     Gets or sets the MIME type of the file.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the upload status of the file.
    /// </summary>
    public UploadStatus UploadStatus { get; set; } = UploadStatus.Ready;

    /// <summary>
    ///     Gets or sets the upload progress of the file as a percentage.
    /// </summary>
    public int UploadProgress { get; set; } = 0;

    /// <summary>
    ///     Gets or sets the browser file data.
    /// </summary>
    public IBrowserFile? FileData { get; set; }
}
