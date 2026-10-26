using DemoApi.Contracts;
using DemoApi.Data;

namespace DemoApi.Auth;

public class AuthService(IUserRepository users, PasswordHasher hasher)
{
    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();

        var existingUser = await users.GetUserByUsernameAsync(username);
        if (existingUser is not null) return null;

        var passwordHash = hasher.Hash(request.Password);
        var user = await users.AddUserAsync(username, passwordHash);

        return new UserResponse(user.Id, user.Username);
    }
}