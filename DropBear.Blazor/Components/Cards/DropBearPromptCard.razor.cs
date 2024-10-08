﻿#region

using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

/// <summary>
///     A Blazor component for rendering a prompt card with various styles and options.
/// </summary>
public sealed partial class DropBearPromptCard : ComponentBase
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

    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;
    [Parameter] public string Icon { get; set; } = "fas fa-question-circle";
    [Parameter] public string Title { get; set; } = "Title";
    [Parameter] public string Description { get; set; } = "Description";
    [Parameter] public IReadOnlyCollection<ButtonConfig> Buttons { get; set; } = Array.Empty<ButtonConfig>();
    [Parameter] public EventCallback<ButtonConfig> OnButtonClicked { get; set; }
    [Parameter] public PromptType PromptType { get; set; } = PromptType.Information;
    [Parameter] public bool Subtle { get; set; }
    private string ThemeClass => Theme is ThemeType.LightMode ? "light-mode" : "";

    /// <summary>
    ///     Gets the CSS class for the specified button color.
    /// </summary>
    /// <param name="type">The button color.</param>
    /// <returns>A string representing the CSS class.</returns>
    private string GetButtonClass(ButtonColor type)
    {
        const string BaseClass = "prompt-btn";
        var typeClass = ButtonClasses.GetValueOrDefault(type, "prompt-btn-default");
#pragma warning disable CA1308
        var promptTypeClass = PromptType.ToString().ToLowerInvariant();
#pragma warning restore CA1308

        return $"{BaseClass} {typeClass} {promptTypeClass}".Trim();
    }

    /// <summary>
    ///     Handles the button click event.
    /// </summary>
    /// <param name="button">The button configuration.</param>
    private Task OnButtonClick(ButtonConfig button)
    {
        return OnButtonClicked.InvokeAsync(button);
    }
}
