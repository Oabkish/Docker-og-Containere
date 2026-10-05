using Npgsql;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");
var app = builder.Build();

if(!string.IsNullOrEmpty(connectionString))
{
    using var conn= new NpgsqlConnection(connectionString);
    conn.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText="""
        CREATE TABLE IF NOT EXISTS items (
            id SERIAL PRIMARY KEY,
            name TEXT NOT NULL,
            created_at TIMESTAMPTZ NOT NULL DEFAULT now()
        );
    """;
    cmd.ExecuteNonQuery();
}



app.MapGet("/health", () => Results.Ok("Ok"));


app.MapPost("/items", async (ItemRequest request) =>
{
await using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();
await using var cmd = conn.CreateCommand();
cmd.CommandText = "INSERT INTO items (name) VALUES (@name) RETURNING id, created_at;";
    cmd.Parameters.AddWithValue("name", request.Name);

    await using var reader = await cmd.ExecuteReaderAsync();
    await reader.ReadAsync();
    var id = reader.GetInt32(0);
    var createdAt = reader.GetDateTime(1);

    return Results.Ok(new ItemResponse(id, request.Name, createdAt));
});

app.MapGet("/items", async () =>
{
    var items = new List<ItemResponse>();

    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT id, name, created_at FROM items ORDER BY id;";

    await using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        items.Add(new ItemResponse(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
    }

    return Results.Ok(items);
});


app.Run();

record ItemRequest(string Name);
record ItemResponse (int Id, string Name, DateTime Created );
