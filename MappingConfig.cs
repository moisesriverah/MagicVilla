using AutoMapper;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;

namespace MagicVillaAPI
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
            CreateMap<NumberVilla, NumberVillaDto>().ReverseMap();
            CreateMap<NumberVilla, NumberVillaCreateDto>().ReverseMap();
            CreateMap<NumberVilla, NumberVillaUpdateDto>().ReverseMap();
        }
    }
}
