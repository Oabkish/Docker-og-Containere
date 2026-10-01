var connectionString = builder.Configuration.GetConnectionString("Postgres");
Using Npgsql;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if(!string.IsNullOrEmpty(connectionString))
{
    using var conn= new NpgsqlConnection(connectionString);
    conn.Open();
    cmd.CommandText="""
    CREATE TABLE IF NOT EXISTS items (
            id SERIAL PRIMARY KEY,
            name TEXT NOT NULL,
            created_at TIMESTAMPTZ NOT NULL DEFAULT now()
        );
        """;
    cmd.ExecuteNonQuery();
}



app.MapGet("/health", () ==> Results.Ok("Ok"));
await using var conn = new NpgsqlConnection(connectionString);
conn.Open();
using var cmd = conn.CreateCommanf();
cmd.CommandText="""
Create Table if not exists items (
id SERIAL PRIMARY KEY,
name TEXT NOT NULL,
created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
""";

cmd.ExecuteNonQuery();

app.MapGet("/health", () => Results.Ok("Ok"));