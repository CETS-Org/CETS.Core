using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_Teacher.Requests;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.IDN
{
    public class IDN_TeacherProfile : Profile
    {
        public IDN_TeacherProfile()
        {
            CreateMap<IDN_Teacher, TeacherResponse>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<CreateTeacherRequest, IDN_Teacher>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId));
       

            CreateMap<IDN_Teacher, UpdateTeacherRequest>()
                .ReverseMap();

            CreateMap<IDN_Teacher, TeacherDetailResponse>()
                // -- Teacher Information --
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.Id))

                // -- Account Information (Flattening) --
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Account.DateOfBirth))
                .ForMember(dest => dest.CID, opt => opt.MapFrom(src => src.Account.CID))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Account.Address))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Account.AvatarUrl))
                .ForMember(dest => dest.AccountStatusID, opt => opt.MapFrom(src => src.Account.AccountStatusID))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.Account.IsVerified))

                // --- Handling Naming Collisions ---
                .ForMember(dest => dest.AccountCreatedAt, opt => opt.MapFrom(src => src.Account.CreatedAt))
                .ForMember(dest => dest.AccountUpdatedAt, opt => opt.MapFrom(src => src.Account.UpdatedAt))
                .ForMember(dest => dest.AccountUpdatedBy, opt => opt.MapFrom(src => src.Account.UpdatedBy))
                .ForMember(dest => dest.AccountIsDeleted, opt => opt.MapFrom(src => src.Account.IsDeleted))

                // -- Teacher Credentials (Mapping a Collection) --
                .ForMember(dest => dest.TeacherCredentials, opt => opt.MapFrom(src => src.IDN_TeacherCredentials));

                // === Child Mapping: TeacherCredential Entity -> TeacherCredentialDetail DTO ===
            CreateMap<IDN_TeacherCredential, TeacherCredentialDetail>()
                .ForMember(dest => dest.CredentialId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CredentialTypeName, opt => opt.MapFrom(src => src.CredentialType.Name));
        }
    }
}
