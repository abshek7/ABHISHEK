using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CapStone.Application.DTOs.Admin;
using CapStone.Domain.Entities;
 

namespace CapStone.Application.Mappings
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<CreateUserDto, User>()
               .ForMember(dest => dest.PasswordHash,
                   opt => opt.MapFrom(src =>
                       BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<CreatePolicyTypeDto, PolicyType>();

            CreateMap<CreatePolicyCoverageDto, PolicyCoverage>();
        }
    }
}