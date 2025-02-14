using AutoMapper;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleDto>().ReverseMap();
            CreateMap<SaleItem, SaleItemDto>().ReverseMap();
        }
    }
}
