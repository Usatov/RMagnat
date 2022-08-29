using AutoMapper;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;

namespace ResourceMagnat.Data;

public class MapProfile: Profile
{
    public MapProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Building, BuildingDto>();
        CreateMap<BuildingType, BuildingTypeDto>();

        CreateMap<BuildingDto, Building>();
    }
}