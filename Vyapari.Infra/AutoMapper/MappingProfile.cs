using AutoMapper;
using Vyapari.Data.Entities;

namespace Vyapari.Infra
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductRequestDto, Product>();
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}