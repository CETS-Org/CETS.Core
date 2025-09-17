using AutoMapper;
using Domain.Entities;
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
            CreateMap<IDN_TeacherCredential, TeacherCredentialResponse>()
                .ForMember(dest => dest.CredentialId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherID))
                .ReverseMap();

            CreateMap<IDN_TeacherCredential, CreateTeacherCredentialRequest>()
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID))
                .ReverseMap();

            CreateMap<IDN_TeacherCredential, UpdateTeacherCredentialRequest>()
                .ForMember(dest => dest.CredentialTypeId, opt => opt.MapFrom(src => src.CredentialTypeID))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherID))
                .ReverseMap();

        }
    }
}
