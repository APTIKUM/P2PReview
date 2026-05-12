using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using P2PReview.Application.Files;
using P2PReview.Application.Interfaces;

namespace P2PReview.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadsAvatarsPath;

        private const long MaxFileSize = 5 * 1024 * 1024;

        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

        public FileService(IWebHostEnvironment env)
        {
            _env = env;

            _uploadsAvatarsPath = Path.Combine(_env.WebRootPath, "uploads", "avatars");

            if (!Directory.Exists(_uploadsAvatarsPath))
            {
                Directory.CreateDirectory(_uploadsAvatarsPath);
            }
        }

        public async Task<string> UploadAvatarAsync(IBrowserFile file)
        {
            if (file is null)
                throw new ArgumentException("File is null");

            if (file.Size == 0)
                throw new ArgumentException("File is empty");

            if (file.Size > MaxFileSize)
                throw new ArgumentException($"File is too large. Max allowed size is {MaxFileSize / 1024 / 1024} MB.");

            var extension = Path.GetExtension(file.Name);

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    $"Invalid file type. Allowed: {string.Join(" ", AllowedExtensions)}");
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadsAvatarsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);

            return fileName;
        }

        public async Task<bool> DeleteAvatarAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var filePath = Path.Combine(_uploadsAvatarsPath, fileName);

            var fullPath = Path.GetFullPath(filePath);

            if (!File.Exists(fullPath))
            {
                return false;
            }

            await Task.Run(() => File.Delete(fullPath));

            return true;
        }

        public async Task<ReadedCodeFileDto> ReadCodeFileAsync(IBrowserFile file, CancellationToken ct = default)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize,
                cancellationToken: ct);
            
            using var reader = new StreamReader(stream);

            var content = await reader.ReadToEndAsync(ct);

            return new ReadedCodeFileDto()
            {
                Name = file.Name,
                Content = content
            };
        }
    }
}