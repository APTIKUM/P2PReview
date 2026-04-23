using Microsoft.AspNetCore.Identity;

public class RussianIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length)
        => new() { Description = $"Пароль должен содержать минимум {length} символов." };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new() { Description = "Пароль должен содержать спецсимвол." };

    public override IdentityError PasswordRequiresDigit()
        => new() { Description = "Пароль должен содержать цифру." };

    public override IdentityError PasswordRequiresLower()
        => new() { Description = "Пароль должен содержать строчную букву." };

    public override IdentityError PasswordRequiresUpper()
        => new() { Description = "Пароль должен содержать заглавную букву." };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        => new() { Description = $"Пароль должен содержать минимум {uniqueChars} уникальных символов." };

    public override IdentityError DuplicateUserName(string userName)
        => new() { Description = "Пользователь с таким email уже существует." };

    public override IdentityError InvalidEmail(string email)
        => new() { Description = "Некорректный email." };
}