using System.ComponentModel.DataAnnotations;

namespace DemoApi.Models;

public class User
{
    [Key]
    public int Id { get; init; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}