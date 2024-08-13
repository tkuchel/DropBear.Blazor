#region

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Buttons;

public partial class DropBearNavigationButtons : ComponentBase
{
    private DotNetObjectReference<DropBearNavigationButtons> objRef;
    private bool IsVisible { get; set; }

    /// <summary>
    ///     Initializes the component and sets up the JavaScript interop.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        objRef = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("DropBearNavigationButtons.initialize", objRef);
    }

    /// <summary>
    ///     Cleans up resources when the component is disposed.
    /// </summary>
    public void Dispose()
    {
        objRef?.Dispose();
    }

    /// <summary>
    ///     Navigates to the previous page in the browser history.
    /// </summary>
    private void GoBack()
    {
        try
        {
            NavigationManager.NavigateTo(NavigationManager.Uri);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error navigating back: {ex.Message}");
            // Consider logging this error or showing a user-friendly message
        }
    }

    /// <summary>
    ///     Navigates to the home page of the application.
    /// </summary>
    private void GoHome()
    {
        try
        {
            NavigationManager.NavigateTo("/");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error navigating home: {ex.Message}");
            // Consider logging this error or showing a user-friendly message
        }
    }

    /// <summary>
    ///     Scrolls the page to the top.
    /// </summary>
    private async Task ScrollToTop()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("DropBearNavigationButtons.scrollToTop");
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error scrolling to top: {ex.Message}");
            // Consider logging this error or showing a user-friendly message
        }
    }

    /// <summary>
    ///     Updates the visibility of the scroll-to-top button.
    ///     This method is called from JavaScript.
    /// </summary>
    /// <param name="isVisible">Whether the button should be visible.</param>
    [JSInvokable]
    public void UpdateVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        StateHasChanged();
    }
}
