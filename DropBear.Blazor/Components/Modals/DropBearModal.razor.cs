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
    private CancellationTokenSource? _dismissCancellationTokenSource;
    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment BodyContent { get; set; } = default!;
    [Parameter] public IReadOnlyCollection<ModalButton> Buttons { get; set; } = Array.Empty<ModalButton>();
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.LightMode;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;

    private bool IsVisible { get; set; }

    public void Dispose()
    {
        ModalService.OnShow -= ShowModal;
        ModalService.OnClose -= CloseModal;

        _dismissCancellationTokenSource?.Dispose();
    }

    protected override void OnInitialized()
    {
        ModalService.OnShow += ShowModal;
        ModalService.OnClose += CloseModal;

        _dismissCancellationTokenSource = new CancellationTokenSource();
    }

    private void ShowModal(object? sender, EventArgs e)
    {
        IsVisible = true;
        StateHasChanged();
    }

    private void CloseModal(object? sender, EventArgs e)
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
        var classes = new List<string> { "modal", Theme is ThemeType.DarkMode ? "theme-dark" : "theme-light" };
        if (IsVisible)
        {
            classes.Add("active");
        }

        return string.Join(' ', classes);
    }

    private static string GetButtonClasses(ButtonColor type)
    {
#pragma warning disable CA1308
        return $"btn btn-{type.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308
    }

    private async Task ToggleTheme()
    {
        Theme = Theme is ThemeType.DarkMode ? ThemeType.LightMode : ThemeType.DarkMode;
        if (_dismissCancellationTokenSource is not null)
        {
            await JSRuntime.InvokeVoidAsync("DropBearModal.updateModalTheme", _dismissCancellationTokenSource.Token,
                "dropBearModal", GetModalClasses());
        }
    }
}
