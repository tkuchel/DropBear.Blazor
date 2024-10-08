﻿namespace DropBear.Blazor.Models;

/// <summary>
///     Represents a file that has been dropped into a file upload control.
/// </summary>
public sealed class DroppedFile
{
    /// <summary>
    ///     Gets or sets the name of the file.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the size of the file in bytes.
    /// </summary>
    public long Size { get; init; }

    /// <summary>
    ///     Gets or sets the MIME type of the file.
    /// </summary>
    public string Type { get; init; } = string.Empty;
}
