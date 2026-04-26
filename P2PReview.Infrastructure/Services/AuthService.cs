using Microsoft.AspNetCore.Identity;
using P2PReview.Application.Auth.Commands;
using P2PReview.Application.Interfaces;
using P2PReview.Domain.Entities;


namespace P2PReview.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> LoginAsync(LoginCommand loginCommand)
        {
            var user = await _userManager.FindByEmailAsync(loginCommand.Email);

            if (user == null)
                return (false, ["Неверный email или пароль"]);

            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginCommand.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return (false, ["Неверный email или пароль"]);

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> RegisterAsync(RegisterCommand registerCommand)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerCommand.Email);

            if (existingUser != null)
                return (false, ["Пользователь с таким email уже существует"]);

            var user = new User
            {
                UserName = registerCommand.Email[..Math.Min(5, registerCommand.Email.IndexOf('@'))],
                Email = registerCommand.Email,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(er => er.Description));

            return (true, null);
        }
    }
}
