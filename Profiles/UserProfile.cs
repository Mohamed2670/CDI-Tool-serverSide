using AutoMapper;
using CDI_Tool.Dtos.UserDtos;
using CDI_Tool.Model;

namespace CDI_Tool.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAddDto>().ReverseMap();
        }
    }
}