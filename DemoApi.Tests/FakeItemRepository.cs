using DemoApi.Data;
using DemoApi.Models;

namespace DemoApi.Tests;

public class FakeItemRepository : IItemRepository
{
    private readonly List<Item> _items = new();
    private int _nextId = 1;

    public Task<Item> AddItemAsync(string name, int userId)
    {
        var item = new Item(name) { Id = _nextId++, UserId = userId };
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<List<Item>> GetItemsAsync(int userId)
    {
        var result = _items.Where(i => i.UserId == userId).OrderBy(i => i.Id).ToList();
        return Task.FromResult(result);
    }

    public Task<Item?> UpdateItemAsync(int id, string name, int userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item is not null) item.Name = name;
        return Task.FromResult(item);
    }

    public Task<bool> DeleteItemAsync(int id, int userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item is null) return Task.FromResult(false);

        _items.Remove(item);
        return Task.FromResult(true);
    }
}