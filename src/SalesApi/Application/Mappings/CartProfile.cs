using AutoMapper;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Application.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
