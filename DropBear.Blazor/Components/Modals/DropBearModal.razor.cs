#region

using DropBear.Blazor.Arguments.Events;
using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Modals;

public sealed partial class DropBearModal : DropBearComponentBase, IDisposable
{
    [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment BodyContent { get; set; } = default!;
    [Parameter] public IReadOnlyCollection<ModalButton> Buttons { get; set; } = Array.Empty<ModalButton>();
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.LightMode;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;

    private bool IsVisible => ModalService.IsModalVisible(Id);

    public void Dispose()
    {
        ModalService.OnShow -= HandleModalShow;
        ModalService.OnClose -= HandleModalClose;
        ModalService.RemoveModal(Id);
    }

    protected override void OnInitialized()
    {
        ModalService.OnShow += HandleModalShow;
        ModalService.OnClose += HandleModalClose;

        // Register this modal with the ModalService
        ModalService.AddModal(new Modal
        {
            Id = Id,
            Title = Title,
            BodyContent = BodyContent,
            Buttons = new List<ModalButton>(Buttons),
            Theme = Theme,
            CloseOnBackdropClick = CloseOnBackdropClick
        });
    }

    private void HandleModalShow(object? sender, ModalEventArgs e)
    {
        if (e.ModalId == Id)
        {
            StateHasChanged();
        }
    }

    private void HandleModalClose(object? sender, ModalEventArgs e)
    {
        if (e.ModalId == Id)
        {
            StateHasChanged();
        }
    }

    private void Close()
    {
        ModalService.Close(Id);
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

    private static string GetButtonClasses(ButtonColor color)
    {
#pragma warning disable CA1308
        return $"btn btn-{color.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308
    }

    private async Task ToggleTheme()
    {
        Theme = Theme == ThemeType.DarkMode ? ThemeType.LightMode : ThemeType.DarkMode;
        await JsRuntime.InvokeVoidAsync("DropBearModal.updateModalTheme", Id, GetModalClasses());
    }
}
