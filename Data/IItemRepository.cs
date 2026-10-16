using DemoApi.Models;

namespace DemoApi.Data;

public interface IItemRepository
{
    Task<Item> AddItemAsync(string name);
    Task<List<Item>> GetItemsAsync();
}