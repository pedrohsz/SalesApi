using AutoMapper;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Application.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, Sale>().ReverseMap();
            CreateMap<SaleItem, SaleItemDto>().ReverseMap();

            CreateMap<Sale, SaleResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemResponse>();
        }
    }
}
