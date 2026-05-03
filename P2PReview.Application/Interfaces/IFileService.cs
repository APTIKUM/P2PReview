using Microsoft.AspNetCore.Components.Forms;

namespace P2PReview.Application.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadAvatarAsync(IBrowserFile file);
    }
}
