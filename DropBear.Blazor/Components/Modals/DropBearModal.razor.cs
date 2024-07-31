#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Modals;

/// <summary>
///     A Blazor component for displaying a modal dialog.
/// </summary>
public sealed partial class DropBearModal : DropBearComponentBase, IDisposable
{
    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment BodyContent { get; set; } = default!;
    [Parameter] public List<ModalButton> Buttons { get; set; } = new();
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.LightMode;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;

    private bool IsVisible { get; set; }

    public void Dispose()
    {
        ModalService.OnShow -= ShowModal;
        ModalService.OnClose -= CloseModal;
    }

    protected override void OnInitialized()
    {
        ModalService.OnShow += ShowModal;
        ModalService.OnClose += CloseModal;
    }

    private void ShowModal()
    {
        IsVisible = true;
        StateHasChanged();
    }

    private void CloseModal()
    {
        IsVisible = false;
        StateHasChanged();
    }

    private void Close()
    {
        ModalService.Close();
    }

    private void CloseModalOnBackdropClick()
    {
        if (CloseOnBackdropClick)
        {
            Close();
        }
    }

    private string GetModalClasses()
    {
        var classes = new List<string> { "modal", Theme == ThemeType.DarkMode ? "theme-dark" : "theme-light" };
        if (IsVisible)
        {
            classes.Add("active");
        }

        return string.Join(" ", classes);
    }

    private static string GetButtonClasses(ButtonColor type)
    {
        return $"btn btn-{type.ToString().ToLower()}";
    }

    private async Task ToggleTheme()
    {
        Theme = Theme == ThemeType.DarkMode ? ThemeType.LightMode : ThemeType.DarkMode;
        await JSRuntime.InvokeVoidAsync("DropBearModal.updateModalTheme", "dropBearModal", GetModalClasses());
    }
}
