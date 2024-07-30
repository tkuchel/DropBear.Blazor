using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace DropBear.Blazor.Models;

public sealed class UploadFile
{
    public string Name { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public UploadStatus UploadStatus { get; set; }
    public int UploadProgress { get; set; }
    public IBrowserFile FileData { get; set; }
}
