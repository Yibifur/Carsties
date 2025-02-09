using Carsties.SearchService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace Carsties.SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        [HttpGet]   
        public async Task<ActionResult<List<Item>>>SearchItem(string searchTerm)
        {
            var query = DB.Find<Item>();
            query.Sort(x=>x.Ascending(a=>a.Make)); 
            if(!string.IsNullOrEmpty(searchTerm))
            {
                query.Match(Search.Full,searchTerm).SortByTextScore();
                
            }
            var result=await query.ExecuteAsync();
            return result;
        }
    }
}
