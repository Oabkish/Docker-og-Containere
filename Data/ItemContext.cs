using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data;

public class ItemContext(DbContextOptions<ItemContext> options) : DbContext(options), IItemRepository
{
    public DbSet<Item> Items { get; set; } = null!;

    public async Task<Item> AddItemAsync(string name)
    {
        var newItem = new Item(name);
        await Items.AddAsync(newItem);
        await SaveChangesAsync();
        return newItem;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        return await Items.OrderBy(i => i.Id).ToListAsync();
    }
}