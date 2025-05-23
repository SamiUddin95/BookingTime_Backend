using Microsoft.AspNetCore.StaticFiles;

namespace BookingTime.Service
{
    public interface IFileLoaderService
    {
        Task<string> LoadFileAsync(string path);
        string GetMimeType(string filePath);
    }
    public class FileLoaderService : IFileLoaderService
    {
        public async Task<string> LoadFileAsync(string path)
        {
            if (!System.IO.File.Exists(path))
                return null;

            var fileBytes = await System.IO.File.ReadAllBytesAsync(path);

            var mimeType = GetMimeType(path);

            var base64String = Convert.ToBase64String(fileBytes);

            return $"data:{mimeType};base64,{base64String}";
        }

        public string GetMimeType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var mimeType))
            {
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }

    }
}
