using AutoMapper;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
