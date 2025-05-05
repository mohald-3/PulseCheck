using Application.DTOs.CheckInDtos;
using Application.DTOs.FriendshipDtos;
using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, User>();

            CreateMap<CheckIn, CheckInDto>();
            CreateMap<CheckInCreateDto, CheckIn>();

            CreateMap<Friendship, FriendshipDto>();
            CreateMap<FriendshipCreateDto, Friendship>();
        }
    }
}
