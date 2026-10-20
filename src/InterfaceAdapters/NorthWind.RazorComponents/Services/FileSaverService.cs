using Microsoft.JSInterop;

namespace NorthWind.RazorComponents.Services;

public class FileSaverService : IFileSaverService
{
    private readonly IJSRuntime _jsRuntime;

    public FileSaverService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask SaveFileAsync(string fileName, string contentType, Stream stream)
    {
        if (stream.CanSeek && stream.Position != 0)
        {
            stream.Position = 0;
        }

        using var streamRef = new DotNetStreamReference(stream);
        await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, contentType, streamRef);
    }

    public async ValueTask SaveFileAsync(string fileName, string contentType, byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        await SaveFileAsync(fileName, contentType, memoryStream);
    }
}
