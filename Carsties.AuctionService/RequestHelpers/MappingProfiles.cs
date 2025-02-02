using AutoMapper;
using Carsties.AuctionService.DTOs;
using Carsties.AuctionService.Entities;

namespace Carsties.AuctionService.RequestHelpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() { 
        
        CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.Item);
        CreateMap<Item,AuctionDto>();
        CreateMap<CreateAuctionDto,Auction>().ForMember(d=>d.Item,o=>o.MapFrom(s=>s));
        CreateMap<CreateAuctionDto, Item>();
        }
    }
}
