#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

/// <summary>
///     A Blazor component for rendering a card with various styles and options.
/// </summary>
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
    private string SelectedTheme => Theme is ThemeType.LightMode ? "light-theme" : "dark-theme";

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

    [Parameter] public IReadOnlyCollection<ButtonConfig> Buttons { get; set; } = Array.Empty<ButtonConfig>();

    [Parameter] public EventCallback<ButtonConfig> OnButtonClicked { get; set; }

    /// <summary>
    ///     Gets the CSS class for the specified button color.
    /// </summary>
    /// <param name="type">The button color.</param>
    /// <returns>A string representing the CSS class.</returns>
    private static string GetButtonClass(ButtonColor type)
    {
        return ButtonClasses.GetValueOrDefault(type, "btn-primary");
    }

    /// <summary>
    ///     Handles the button click event.
    /// </summary>
    /// <param name="button">The button configuration.</param>
    private async Task OnButtonClick(ButtonConfig button)
    {
        await OnButtonClicked.InvokeAsync(button);
    }
}
