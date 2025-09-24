using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_Teacher.Requests;
using DTOs.IDN.IDN_TeacherCredential.Requests;
using DTOs.IDN.IDN_TeacherCredential.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.IDN
{
    public class IDN_TeacherCredentialProfile : Profile
    {
        public IDN_TeacherCredentialProfile()
        {
            // Entity -> Response
            CreateMap<IDN_TeacherCredential, TeacherCredentialResponse>()
                .ForMember(dest => dest.CredentialId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherID));

            // Entity -> Create DTO
            CreateMap<IDN_TeacherCredential, CreateTeacherCredentialRequest>()
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID));

            // Entity -> Update DTO
            CreateMap<IDN_TeacherCredential, UpdateTeacherCredentialRequest>()
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID));

            // Update DTO -> Entity
            CreateMap<UpdateTeacherCredentialRequest, IDN_TeacherCredential>()
                .ForMember(dest => dest.CredentialTypeID, opt => opt.MapFrom(src => src.CredentialTypeId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            // Create DTO -> Entity
            CreateMap<CreateTeacherCredentialRequest, IDN_TeacherCredential>()
             .ForMember(dest => dest.CredentialTypeID, opt => opt.MapFrom(src => src.CredentialTypeId))
             .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
