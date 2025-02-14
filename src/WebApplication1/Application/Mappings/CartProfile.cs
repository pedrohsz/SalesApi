using AutoMapper;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mappings
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
