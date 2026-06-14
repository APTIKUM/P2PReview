using P2PReview.Application.Auth.Commands;

namespace P2PReview.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<(bool Success, IEnumerable<string>? Errors)> LoginAsync(LoginCommand loginCommand);
        public Task<(bool Success, IEnumerable<string>? Errors)> RegisterAsync(RegisterCommand registerCommand);
        public Task<bool> Logout();
    }
}
