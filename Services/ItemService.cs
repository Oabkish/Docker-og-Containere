using DemoApi.Contracts;
using DemoApi.Data;

namespace DemoApi.Services;

public class ItemService(IItemRepository repository)
{
    public async Task<ItemResponse> AddItemAsync(ItemRequest request, int userId)
    {
        var item = await repository.AddItemAsync(request.Name, userId);
        return new ItemResponse(item.Id, item.Name, item.CreatedAt);
    }

    public async Task<List<ItemResponse>> GetItemsAsync(int userId)
    {
        var items = await repository.GetItemsAsync(userId);
        return items.Select(i => new ItemResponse(i.Id, i.Name, i.CreatedAt)).ToList();
    }

    public async Task<ItemResponse?> UpdateItemAsync(int id, ItemRequest request, int userId)
    {
        var item = await repository.UpdateItemAsync(id, request.Name, userId);
        if (item is null) return null;

        return new ItemResponse(item.Id, item.Name, item.CreatedAt);
    }

    public async Task<bool> DeleteItemAsync(int id, int userId)
    {
        return await repository.DeleteItemAsync(id, userId);
    }
}