using System.ComponentModel.DataAnnotations;

namespace DemoApi.Models;

public class Item(string name)
{
    [Key]
    public int Id { get; init; }

    public string Name { get; set; } = name;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}