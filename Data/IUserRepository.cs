using DemoApi.Models;

namespace DemoApi.Data;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> AddUserAsync(string username, string passwordHash);
}