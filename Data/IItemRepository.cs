using DemoApi.Models;

namespace DemoApi.Data;

public interface IItemRepository
{
    Task<Item> AddItemAsync(string name, int userId);
    Task<List<Item>> GetItemsAsync(int userId);
    Task<Item?> UpdateItemAsync(int id, string name, int userId);
    Task<bool> DeleteItemAsync(int id, int userId);
}