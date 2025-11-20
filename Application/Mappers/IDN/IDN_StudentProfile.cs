using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Student.Requests;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.IDN
{
    public class IDN_StudentProfile : Profile
    {
        public IDN_StudentProfile() 
        {
            CreateMap<IDN_Student, StudentResponse>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<IDN_Student, WaitingStudentResponse>()
               .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.StudentCode))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.PhoneNumber))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
               .ReverseMap();

            CreateMap<CreateStudentRequest, IDN_Student>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId));

            CreateMap<IDN_Student, UpdateStudentRequest>()
                .ReverseMap();

            // UpdateStudentRequest -> IDN_Student
            CreateMap<UpdateStudentRequest, IDN_Student>() 
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateStudentProfileRequest, IDN_Student>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateStudentProfileRequest, IDN_Account>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // IDN_Student -> StudentProfileResponse
            CreateMap<IDN_Student, StudentProfileResponse>()
                .ForMember(dest => dest.AccountID, opt => opt.MapFrom(src => src.Account.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
                .ForMember(dest => dest.CID, opt => opt.MapFrom(src => src.Account.CID))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Account.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Account.DateOfBirth))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Account.AvatarUrl));

        }
    }
}
