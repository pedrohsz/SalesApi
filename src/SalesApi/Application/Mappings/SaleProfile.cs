using AutoMapper;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Application.Mappings
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
