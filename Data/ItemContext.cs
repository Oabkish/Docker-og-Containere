using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data;

public class ItemContext(DbContextOptions<ItemContext> options) : DbContext(options), IItemRepository
{
    public DbSet<Item> Items { get; set; } = null!;

    public async Task<Item> AddItemAsync(string name, int userId)
    {
        var newItem = new Item(name) { UserId = userId };
        await Items.AddAsync(newItem);
        await SaveChangesAsync();
        return newItem;
    }

    public async Task<List<Item>> GetItemsAsync(int userId)
    {
        return await Items
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<Item?> UpdateItemAsync(int id, string name, int userId)
    {
        var item = await Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (item is null) return null;

        item.Name = name;
        await SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteItemAsync(int id, int userId)
    {
        var item = await Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (item is null) return false;

        Items.Remove(item);
        await SaveChangesAsync();
        return true;
    }
}