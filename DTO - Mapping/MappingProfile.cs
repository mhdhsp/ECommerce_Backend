using AutoMapper;
using ECommerceBackend.Models;

namespace ECommerceBackend.DTO___Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserReqDto, UserModel>().ReverseMap();
            CreateMap<CartItemReqDto, CartItemModel>();
            CreateMap<WishListItemReqDto, WishListModel>();
            CreateMap<UserModel, UsersResDto>();
        }

    }
}
