using DemoApi.Contracts;
using DemoApi.Data;

namespace DemoApi.Auth;

public class AuthService(IUserRepository users, PasswordHasher hasher, JwtService jwtService)
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

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();

        var user = await users.GetUserByUsernameAsync(username);
        if (user is null) return null;

        var passwordIsCorrect = hasher.Verify(request.Password, user.PasswordHash);
        if (!passwordIsCorrect) return null;

        var token = jwtService.CreateToken(user);
        return new LoginResponse(token);
    }
}