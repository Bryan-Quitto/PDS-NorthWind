namespace NorthWind.RazorComponents.Services;

public interface IFileSaverService
{
    ValueTask SaveFileAsync(string fileName, string contentType, byte[] data);
    ValueTask SaveFileAsync(string fileName, string contentType, Stream stream);
}
