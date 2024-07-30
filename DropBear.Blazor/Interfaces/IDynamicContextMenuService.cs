#region

using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

public interface IDynamicContextMenuService
{
    Task<List<ContextMenuItem>> GetMenuItemsAsync(object context, string menuType);
}
