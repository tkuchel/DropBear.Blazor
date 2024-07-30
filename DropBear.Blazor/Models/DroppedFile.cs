namespace DropBear.Blazor.Models;

public sealed class DroppedFile
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; } = 0;
    public string Type { get; set; } = string.Empty;
}
