namespace DemoApi.Contracts;

public record RegisterRequest(string Username, string Password);

public record UserResponse(int Id, string Username);

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token);