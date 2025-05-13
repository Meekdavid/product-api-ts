using Microsoft.Extensions.Options;
using TrueStoryAPI.Helpers.Config;
using TrueStoryAPI.Interfaces;
using TrueStoryAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration for Mock API
builder.Services.Configure<MockApiOptions>(builder.Configuration.GetSection("MockApi"));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register interface and repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register repository with HTTP client, using BaseUrl from configuration
builder.Services.AddHttpClient<IProductRepository, ProductRepository>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<MockApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();