using DemoApi.Contracts;
using DemoApi.Data;
using DemoApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<IItemRepository, ItemContext>(options =>
{
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<ItemService>();

var app = builder.Build();

if (!string.IsNullOrEmpty(connectionString))
{
    using var scope = app.Services.CreateScope();
    var context = (ItemContext)scope.ServiceProvider.GetRequiredService<IItemRepository>();
    context.Database.Migrate();
}

app.MapGet("/health", () => Results.Ok("Ok"));

app.MapPost("/items", async (ItemRequest request, ItemService service) =>
{
    var response = await service.AddItemAsync(request);
    return Results.Ok(response);
});

app.MapGet("/items", async (ItemService service) =>
{
    var items = await service.GetItemsAsync();
    return Results.Ok(items);
});

app.Run();