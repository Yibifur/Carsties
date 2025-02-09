using Carsties.SearchService.Models;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Text.Json;

namespace Carsties.SearchService.Data
{
    public class DbInitializer
    {
        public  static async Task InitDb(WebApplication app)
        {
           
            await DB.InitAsync("SearchDb", MongoClientSettings.
                FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
            await DB.Index<Item>()
                .Key(a => a.Make, KeyType.Text)
                .Key(a => a.Model, KeyType.Text)
                .Key(a => a.Year, KeyType.Text)
                .CreateAsync();
            var count=await DB.CountAsync<Item>();
            if (count == 0)
            {
                try
                {
                    var itemData = await File.ReadAllTextAsync("Data/Auctions.json");
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var itemsList = JsonSerializer.Deserialize<List<Item>>(itemData, options);
                    await DB.SaveAsync(itemsList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}
