using AutoMapper;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
