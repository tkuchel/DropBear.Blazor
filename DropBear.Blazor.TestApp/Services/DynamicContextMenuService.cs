using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Models;

namespace DropBear.Blazor.TestApp.Services;

public class DynamicContextMenuService : IDynamicContextMenuService
{
    public Task<List<ContextMenuItem>> GetMenuItemsAsync(object context, string menuType)
    {
        throw new NotImplementedException();
    }
}
