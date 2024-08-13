#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Containers;

public sealed partial class SectionComponent : ComponentBase
{
    /// <summary>
    ///     A predicate function that determines whether the section should be rendered.
    ///     If the predicate is null or returns true, the section will be rendered.
    /// </summary>
    [Parameter]
    public Func<bool>? Predicate { get; set; }

    /// <summary>
    ///     The content to be rendered within the section.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Validates the component's parameters and initializes necessary properties.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Ensure the ChildContent is not null
        if (ChildContent is null)
        {
            throw new InvalidOperationException("ChildContent must be provided and cannot be null.");
        }
    }
}
