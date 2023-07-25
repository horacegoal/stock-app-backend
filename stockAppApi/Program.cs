using Microsoft.EntityFrameworkCore;
using stockAppApi.BLL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add DataContext class to the services container
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDatabase")));

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IScrapStockDataService, ScrapStockDataService>();
builder.Services.AddScoped<IYahooScrapStockDataService, YahooScrapStockDataService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
Console.WriteLine("app.Environment.IsDevelopment()");
Console.WriteLine(app.Environment.IsDevelopment());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// return Hello world from root path
app.MapGet("/", () =>
{
    return Results.Ok("Hello Worlds!");
});
Console.WriteLine("Hello Worldss!");

app.Run();

