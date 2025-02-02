using AutoMapper;
using Carsties.AuctionService.Data;
using Carsties.AuctionService.DTOs;
using Carsties.AuctionService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carsties.AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext context;
        private readonly IMapper mapper;

        public AuctionsController(AuctionDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAuctions()
        {
            var auctions=await context.Auctions.Include(x=>x.Item).
                OrderBy(x=>x.Item.Make).ToListAsync();
            var auctiondtos=mapper.Map<List<AuctionDto>>(auctions);
            return Ok(auctiondtos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x=>x.Id==id);
              if (auction == null)
                    return NotFound();
            var auctiondto = mapper.Map<AuctionDto>(auction);
            return Ok(auctiondto);
        }
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateNewAuction([FromBody] CreateAuctionDto model)
        {
            var auction=mapper.Map<Auction>(model);
            auction.Seller = "test";
            context.Auctions.Add(auction);
           var result= await context.SaveChangesAsync()>0;
            if (result == false) { return BadRequest("Could not save changes to the DB"); }
            return CreatedAtAction(nameof(GetAuctionById), new {auction.Id},mapper.Map<AuctionDto>(auction));

        }
        [HttpPut]
        public async Task<ActionResult> UpdateAuction(Guid id,[FromBody] UpdateAuctionDto model)
        {
            var auction = await context.Auctions.Include(x=>x.Item).FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) { return NotFound(); } 
            //TODO: check seller=username
            auction.Item.Make = model.Make ?? auction.Item.Make;
            auction.Item.Model = model.Model ?? auction.Item.Model;
            auction.Item.Color = model.Color ?? auction.Item.Color;
            auction.Item.Mileage = model.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = model.Year ?? auction.Item.Year;
            var result = await context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(result);  
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction=await context.Auctions.FindAsync(id);
            if (auction == null) { return NotFound(); }
            //TODO: check seller=username
            context.Auctions.Remove(auction);
            var result = await context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest("Could not update to the DB");
        }
    }
}
