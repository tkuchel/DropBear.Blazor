#region

using DropBear.Blazor.Components.Bases;
using DropBear.Codex.Validation.ReturnTypes;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Validation;

public sealed partial class ValidationErrorsComponent : DropBearComponentBase
{
    private bool _isCollapsed;

    [Parameter] public ValidationResult? ValidationResult { get; set; }

    private void ToggleCollapse()
    {
        _isCollapsed = !_isCollapsed;
    }
}
