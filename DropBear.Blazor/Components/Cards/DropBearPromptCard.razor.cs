#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class DropBearPromptCard : ComponentBase
{
    private static readonly Dictionary<ButtonColor, string> ButtonClasses = new()
    {
        { ButtonColor.Primary, "prompt-btn-primary" },
        { ButtonColor.Secondary, "prompt-btn-secondary" },
        { ButtonColor.Success, "prompt-btn-success" },
        { ButtonColor.Warning, "prompt-btn-warning" },
        { ButtonColor.Error, "prompt-btn-danger" },
        { ButtonColor.Default, "prompt-btn-default" }
    };

    private static readonly Dictionary<ButtonColor, string> GradientTypes = new()
    {
        { ButtonColor.Default, "confirmation" },
        { ButtonColor.Primary, "information" },
        { ButtonColor.Secondary, "confirmation" },
        { ButtonColor.Success, "success" },
        { ButtonColor.Warning, "warning" },
        { ButtonColor.Error, "error" }
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

    private string GetButtonClass(ButtonColor type)
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
