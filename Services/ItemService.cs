using DemoApi.Contracts;
using DemoApi.Data;

namespace DemoApi.Services;

public class ItemService(IItemRepository repository)
{
    public async Task<ItemResponse> AddItemAsync(ItemRequest request)
    {
        var item = await repository.AddItemAsync(request.Name);
        return new ItemResponse(item.Id, item.Name, item.CreatedAt);
    }

    public async Task<List<ItemResponse>> GetItemsAsync()
    {
        var items = await repository.GetItemsAsync();
        return items.Select(i => new ItemResponse(i.Id, i.Name, i.CreatedAt)).ToList();
    }
}