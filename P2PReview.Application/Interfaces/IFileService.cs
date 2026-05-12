using Microsoft.AspNetCore.Components.Forms;
using P2PReview.Application.Files;

namespace P2PReview.Application.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadAvatarAsync(IBrowserFile file);
        public Task<bool> DeleteAvatarAsync(string avatarId);

        public Task<ReadedCodeFileDto> ReadCodeFileAsync(IBrowserFile file,
            CancellationToken ct = default);
    }
}
