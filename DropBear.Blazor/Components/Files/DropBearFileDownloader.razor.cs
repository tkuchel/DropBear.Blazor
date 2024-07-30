#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Files;

public partial class DropBearFileDownloader : DropBearComponentBase
{
    private int _downloadProgress = 0;

    private bool _isDownloading = false;

    [Parameter] public string FileName { get; set; } = "example_document.pdf";
    [Parameter] public string FileSize { get; set; } = "2.5 MB";
    [Parameter] public string FileIconClass { get; set; } = "fas fa-file-pdf";
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;

    [Parameter] public Func<Stream, IProgress<int>, Task<MemoryStream>> DownloadFileAsync { get; set; }
    [Parameter] public EventCallback<bool> OnDownloadComplete { get; set; }

    private string ThemeClass => Theme == ThemeType.LightMode ? "light-theme" : "dark-theme";

    private async Task StartDownload()
    {
        if (_isDownloading || DownloadFileAsync == null)
        {
            return;
        }

        _isDownloading = true;
        _downloadProgress = 0;

        try
        {
            var progress = new Progress<int>(percent =>
            {
                _downloadProgress = percent;
                StateHasChanged();
            });

            using var downloadStream = new MemoryStream();
            using var resultStream = await DownloadFileAsync(downloadStream, progress);

            resultStream.Position = 0;
            var byteArray = resultStream.ToArray();

            await JSRuntime.InvokeVoidAsync("downloadFileFromStream", FileName, byteArray);

            await OnDownloadComplete.InvokeAsync(true);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Download failed: {ex.Message}");
            await OnDownloadComplete.InvokeAsync(false);
        }
        finally
        {
            _isDownloading = false;
            StateHasChanged();
        }
    }
}
