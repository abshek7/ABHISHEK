using AutoMapper;
using CapStone.Application.DTOs.Claim;
using CapStone.Application.DTOs.Customer;
using CapStone.Domain.Entities;

namespace CapStone.Application.Mappings
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<InsuranceClaim, ClaimDetailResponseDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
                .ForMember(d => d.PolicyNumber, o => o.MapFrom(s => s.Policy != null ? s.Policy.PolicyNumber : null));

            CreateMap<InsuranceClaim, ClaimResponseDto>();
        }
    }
}
