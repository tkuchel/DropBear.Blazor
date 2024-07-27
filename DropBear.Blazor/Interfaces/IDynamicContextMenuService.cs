using DropBear.Blazor.Models;

namespace DropBear.Blazor.Interfaces;

public interface IDynamicContextMenuService
{
    Task<List<ContextMenuItem>> GetMenuItemsAsync(object context, string menuType);
}
