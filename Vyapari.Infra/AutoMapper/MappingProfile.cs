using AutoMapper;
using Vyapari.Data.Entities;

namespace Vyapari.Infra
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductRequest, Product>();
        }
    }
}