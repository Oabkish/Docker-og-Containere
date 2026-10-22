using System.Text;
using DemoApi.Contracts;
using DemoApi.Data;
using DemoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<IItemRepository, ItemContext>(options =>
{
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<ItemService>();

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is missing in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste your JWT token here (without the word Bearer)"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

if (!string.IsNullOrEmpty(connectionString))
{
    using var scope = app.Services.CreateScope();
    var context = (ItemContext)scope.ServiceProvider.GetRequiredService<IItemRepository>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

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