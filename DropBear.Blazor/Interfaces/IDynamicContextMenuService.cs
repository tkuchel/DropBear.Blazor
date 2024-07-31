#region

using DropBear.Blazor.Models;

#endregion

namespace DropBear.Blazor.Interfaces;

/// <summary>
///     Interface for a service that provides dynamic context menu items.
/// </summary>
public interface IDynamicContextMenuService
{
    /// <summary>
    ///     Gets the menu items asynchronously based on the provided context and menu type.
    /// </summary>
    /// <param name="context">The context for which the menu items are requested.</param>
    /// <param name="menuType">The type of the menu.</param>
    /// <returns>A task representing the asynchronous operation, with a list of context menu items as the result.</returns>
    Task<List<ContextMenuItem>> GetMenuItemsAsync(object context, string menuType);
}
