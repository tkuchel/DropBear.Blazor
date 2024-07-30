#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class DropBearPromptCard : ComponentBase
{
    private static readonly Dictionary<ButtonType, string> ButtonClasses = new()
    {
        { ButtonType.Primary, "prompt-btn-primary" },
        { ButtonType.Secondary, "prompt-btn-secondary" },
        { ButtonType.Success, "prompt-btn-success" },
        { ButtonType.Warning, "prompt-btn-warning" },
        { ButtonType.Error, "prompt-btn-danger" },
        { ButtonType.Default, "prompt-btn-default" }
    };

    private static readonly Dictionary<ButtonType, string> GradientTypes = new()
    {
        { ButtonType.Default, "confirmation" },
        { ButtonType.Primary, "information" },
        { ButtonType.Secondary, "confirmation" },
        { ButtonType.Success, "success" },
        { ButtonType.Warning, "warning" },
        { ButtonType.Error, "error" }
    };

    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Icon { get; set; } = "fas fa-question-circle";
    [Parameter] public string Title { get; set; } = "Title";
    [Parameter] public string Description { get; set; } = "Description";
    [Parameter] public List<ButtonConfig> Buttons { get; set; } = new();
    [Parameter] public EventCallback<ButtonConfig> OnButtonClicked { get; set; }
    [Parameter] public PromptType PromptType { get; set; } = PromptType.Information;
    [Parameter] public bool Subtle { get; set; }
    private string ThemeClass => Theme == ThemeType.LightMode ? "light-mode" : "";

    private string GetButtonClass(ButtonType type)
    {
        var baseClass = "prompt-btn";
        var typeClass = ButtonClasses.GetValueOrDefault(type, "prompt-btn-default");
        var promptTypeClass = PromptType.ToString().ToLower();

        return $"{baseClass} {typeClass} {promptTypeClass}".Trim();
    }

    private Task OnButtonClick(ButtonConfig button)
    {
        return OnButtonClicked.InvokeAsync(button);
    }
}
