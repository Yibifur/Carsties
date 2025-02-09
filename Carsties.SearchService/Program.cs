using Carsties.SearchService.Data;
using Carsties.SearchService.Models;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.




app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.InitDb(app);    
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
app.Run();
