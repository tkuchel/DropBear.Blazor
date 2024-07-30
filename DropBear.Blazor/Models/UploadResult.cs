using DropBear.Blazor.Enums;

namespace DropBear.Blazor.Models;

public sealed class UploadResult
{
    public UploadStatus Status { get; set; }
    public string Message { get; set; }
}
