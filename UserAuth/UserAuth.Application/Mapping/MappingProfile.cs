using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Application.DTOs.User;
using UserAuth.Domain.Models;

namespace UserAuth.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<UserCreateDTO, User>();
            CreateMap<LoginDTO, User>();
            CreateMap<User, UserResponseDTO>()
                .ForMember(
                    dest => dest.CreatedByName,
                    opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Name : string.Empty)
                );
        }
    }
}
