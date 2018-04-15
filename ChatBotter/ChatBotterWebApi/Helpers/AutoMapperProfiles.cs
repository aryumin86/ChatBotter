using System;
using AutoMapper;
using CBLib.Entities;
using ChatBotterWebApi.DTO;

namespace ChatBotterWebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
