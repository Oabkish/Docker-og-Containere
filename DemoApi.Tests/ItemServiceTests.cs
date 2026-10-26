using DemoApi.Contracts;
using DemoApi.Services;
using Xunit;

namespace DemoApi.Tests;

public class ItemServiceTests
{
    [Fact]
    public async Task GetItems_ReturnsOnlyTheUsersOwnItems()
    {
        var service = new ItemService(new FakeItemRepository());
        await service.AddItemAsync(new ItemRequest("Alice item"), 1);
        await service.AddItemAsync(new ItemRequest("Bob item"), 2);

        var items = await service.GetItemsAsync(1);

        Assert.Single(items);
        Assert.Equal("Alice item", items[0].Name);
    }

    [Fact]
    public async Task UpdateItem_ReturnsNull_WhenItemBelongsToAnotherUser()
    {
        var service = new ItemService(new FakeItemRepository());
        var aliceItem = await service.AddItemAsync(new ItemRequest("Alice item"), 1);

        var result = await service.UpdateItemAsync(aliceItem.Id, new ItemRequest("Hacked"), 2);

        Assert.Null(result);
        var aliceItems = await service.GetItemsAsync(1);
        Assert.Equal("Alice item", aliceItems[0].Name);
    }

    [Fact]
    public async Task DeleteItem_ReturnsFalse_WhenItemBelongsToAnotherUser()
    {
        var service = new ItemService(new FakeItemRepository());
        var aliceItem = await service.AddItemAsync(new ItemRequest("Alice item"), 1);

        var deleted = await service.DeleteItemAsync(aliceItem.Id, 2);

        Assert.False(deleted);
        Assert.Single(await service.GetItemsAsync(1));
    }

    [Fact]
    public async Task DeleteItem_ReturnsTrue_WhenItemBelongsToTheUser()
    {
        var service = new ItemService(new FakeItemRepository());
        var aliceItem = await service.AddItemAsync(new ItemRequest("Alice item"), 1);

        var deleted = await service.DeleteItemAsync(aliceItem.Id, 1);

        Assert.True(deleted);
        Assert.Empty(await service.GetItemsAsync(1));
    }
}