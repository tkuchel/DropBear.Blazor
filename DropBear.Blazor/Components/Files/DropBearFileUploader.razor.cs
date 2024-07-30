#region

using DropBear.Blazor.Components.Bases;
using DropBear.Blazor.Enums;
using DropBear.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Files;

public sealed partial class DropBearFileUploader : DropBearComponentBase
{
    private bool _isDragOver = false;
    private bool _isUploading = false;
    private List<UploadFile> _selectedFiles = new();
    private List<UploadFile> _uploadedFiles = new();
    private int _uploadProgress = 0;

    [Parameter] public int MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB default

    [Parameter] public List<string> AllowedFileTypes { get; set; } = new();

    [Parameter] public EventCallback<List<UploadFile>> OnFilesUploaded { get; set; }

    [Parameter] public ThemeType Theme { get; set; } = ThemeType.DarkMode;

    [Parameter] public Func<UploadFile, IProgress<int>, Task<UploadResult>> UploadFileAsync { get; set; }


    private string GetThemeClass()
    {
        return Theme == ThemeType.LightMode ? "light-theme" : "dark-theme";
    }

    private async Task HandleDrop()
    {
        _isDragOver = false;
        await HandleDroppedFiles();
    }

    private async Task HandleDroppedFiles()
    {
        var files = await JSRuntime.InvokeAsync<List<DroppedFile>>("DropBearFileUploader.getDroppedFiles");
        foreach (var file in files)
        {
            if (IsFileValid(file))
            {
                var uploadFile = new UploadFile
                {
                    Name = file.Name, Size = file.Size, ContentType = file.Type, UploadStatus = UploadStatus.Ready
                };

                _selectedFiles.Add(uploadFile);
            }
        }

        await JSRuntime.InvokeVoidAsync("DropBearFileUploader.clearDroppedFiles");
        StateHasChanged();
    }

    private async Task HandleFileSelection(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            if (IsFileValid(file))
            {
                var uploadFile = new UploadFile
                {
                    Name = file.Name,
                    Size = file.Size,
                    ContentType = file.ContentType,
                    UploadStatus = UploadStatus.Ready,
                    FileData = file
                };

                _selectedFiles.Add(uploadFile);
            }
        }

        StateHasChanged();
    }

    private bool IsFileValid(IBrowserFile file)
    {
        return IsFileValid(new DroppedFile { Name = file.Name, Size = file.Size, Type = file.ContentType });
    }

    private bool IsFileValid(DroppedFile file)
    {
        if (file.Size > MaxFileSize)
        {
            // You might want to show an error message to the user here
            return false;
        }

        if (AllowedFileTypes.Any() && !AllowedFileTypes.Contains(file.Type))
        {
            // You might want to show an error message to the user here
            return false;
        }

        return true;
    }

    private void RemoveFile(UploadFile file)
    {
        _selectedFiles.Remove(file);
        StateHasChanged();
    }

    private async Task UploadFiles()
    {
        _isUploading = true;
        _uploadProgress = 0;

        for (var i = 0; i < _selectedFiles.Count; i++)
        {
            var file = _selectedFiles[i];
            file.UploadStatus = UploadStatus.Uploading;

            try
            {
                if (UploadFileAsync != null)
                {
                    var progress = new Progress<int>(percent =>
                    {
                        file.UploadProgress = percent;
                        _uploadProgress = (int)(_selectedFiles.Sum(f => f.UploadProgress) / (float)_selectedFiles.Count);
                        StateHasChanged();
                    });

                    var result = await UploadFileAsync(file, progress);
                    file.UploadStatus = result.Status;
                    if (result.Status == UploadStatus.Success)
                    {
                        _uploadedFiles.Add(file);
                    }
                }
                else
                {
                    // Fallback to simulated upload if no upload function is provided
                    await Task.Delay(1000);
                    file.UploadStatus = Random.Shared.Next(10) < 8 ? UploadStatus.Success : UploadStatus.Failure;
                    if (file.UploadStatus == UploadStatus.Success)
                    {
                        _uploadedFiles.Add(file);
                    }
                }
            }
            catch
            {
                file.UploadStatus = UploadStatus.Failure;
            }

            _uploadProgress = (int)((i + 1) / (float)_selectedFiles.Count * 100);
            StateHasChanged();
        }

        await OnFilesUploaded.InvokeAsync(_uploadedFiles);

        _isUploading = false;
        _uploadProgress = 100;

        // Remove successfully uploaded files from the selected files list
        _selectedFiles.RemoveAll(f => f.UploadStatus == UploadStatus.Success);

        StateHasChanged();
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        var order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes = bytes / 1024;
        }

        return $"{bytes:0.##} {sizes[order]}";
    }

    private string GetFileStatusClass(UploadStatus status)
    {
        return status switch
        {
            UploadStatus.Ready => "file-status-ready",
            UploadStatus.Uploading => "file-status-uploading",
            UploadStatus.Success => "file-status-success",
            UploadStatus.Failure => "file-status-failure",
            UploadStatus.Warning => "file-status-warning",
            _ => ""
        };
    }

    private string GetFileStatusIconClass(UploadStatus status)
    {
        return status switch
        {
            UploadStatus.Success => "fas fa-check-circle text-success",
            UploadStatus.Failure => "fas fa-times-circle text-danger",
            UploadStatus.Warning => "fas fa-exclamation-circle text-warning",
            _ => "fas fa-question-circle text-muted"
        };
    }
}
