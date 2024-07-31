#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class DropBearCard : ComponentBase
{
    private static readonly Dictionary<ButtonColor, string> ButtonClasses = new()
    {
        { ButtonColor.Default, "btn-primary" },
        { ButtonColor.Secondary, "btn-secondary" },
        { ButtonColor.Success, "btn-success" },
        { ButtonColor.Warning, "btn-warning" }
    };

    [Parameter] public ThemeType Theme { get; set; } = ThemeType.LightMode;
    private string SelectedTheme => Theme == ThemeType.LightMode ? "light-theme" : "dark-theme";

    [Parameter] public CardType Type { get; set; } = CardType.Default;

    [Parameter] public bool CompactMode { get; set; }
    private string CompactSelected => CompactMode ? "compact" : string.Empty;

    [Parameter] public string ImageSource { get; set; } = string.Empty;
    [Parameter] public string ImageAlt { get; set; } = string.Empty;

    [Parameter] public string IconSource { get; set; } = string.Empty;

    [Parameter] public string HeaderTitle { get; set; } = string.Empty;

    [Parameter] public RenderFragment? CardBodyContent { get; set; }

    [Parameter] public bool UseCustomFooter { get; set; }
    [Parameter] public RenderFragment? CardFooterContent { get; set; }

    [Parameter] public List<ButtonConfig> Buttons { get; set; } = new();

    [Parameter] public EventCallback<ButtonConfig> OnButtonClicked { get; set; }

    private static string GetButtonClass(ButtonColor type)
    {
        return ButtonClasses.GetValueOrDefault(type, "btn-primary");
    }

    private async Task OnButtonClick(ButtonConfig button)
    {
        await OnButtonClicked.InvokeAsync(button);
    }
}
