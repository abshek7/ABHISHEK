using AutoMapper;
using CapStone.Application.DTOs.Customer;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;

namespace CapStone.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreatePolicyRequestDto, PolicyRequest>()
                .ForMember(d => d.CustomerId, o => o.Ignore())
                .ForMember(d => d.RequestDate, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.AssignedAgentId, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AssignedAgent, o => o.Ignore())
                .ForMember(d => d.PolicyType, o => o.Ignore());

            CreateMap<PolicyRequest, PolicyRequestResponseDto>()
                .ForMember(d => d.PolicyTypeName, o => o.MapFrom(s => s.PolicyType != null ? s.PolicyType.Name : string.Empty))
                .ForMember(d => d.AssignedAgentName, o => o.MapFrom(s => s.AssignedAgent != null ? s.AssignedAgent.Name : null))
                .ForMember(d => d.Status, o => o.MapFrom(s => Enum.GetName(typeof(RequestStatus), s.Status)));

            CreateMap<Policy, PolicyResponseDto>()
                .ForMember(d => d.PolicyTypeName, o => o.MapFrom(s => s.PolicyType != null ? s.PolicyType.Name : string.Empty));

            CreateMap<CreateClaimDto, InsuranceClaim>()
                .ForMember(d => d.CustomerId, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Status, o => o.MapFrom(_ => ClaimStatus.Submitted))
                .ForMember(d => d.ApprovedAmount, o => o.Ignore())
                .ForMember(d => d.OfficerId, o => o.Ignore())
                .ForMember(d => d.Policy, o => o.Ignore())
                .ForMember(d => d.Customer, o => o.Ignore())
                .ForMember(d => d.Officer, o => o.Ignore());

            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(d => d.PaymentDate, o => o.MapFrom(s => s.PaymentDate));

            CreateMap<CreateNomineeDto, Nominee>()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<Nominee, NomineeResponseDto>();

            CreateMap<PolicyType, PolicyTypeResponseDto>();

        }
    }
}
