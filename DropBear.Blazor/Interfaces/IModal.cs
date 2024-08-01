#region

using DropBear.Blazor.Enums;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface IModal
{
    string Id { get; }
    string Title { get; }
    ThemeType Theme { get; }
    bool IsVisible { get; set; }
    object Context { get; }
}
