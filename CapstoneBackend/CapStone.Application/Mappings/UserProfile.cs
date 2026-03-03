using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using CapStone.Application.DTOs.Auth;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;

namespace CapStone.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.MapFrom(src =>
                        BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => UserRole.Customer))
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}