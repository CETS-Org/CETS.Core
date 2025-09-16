using AutoMapper;
using Domain.Entities;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using DTOs.IDN.IDN_Student.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.IDN
{
    public class IDN_AccountProfile : Profile
    {
        public IDN_AccountProfile()
        {
            CreateMap<IDN_Account, AccountResponse>()
                .ForMember(dest => dest.AccountId,
                    opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.StatusName,
                    opt => opt.MapFrom(src => src.AccountStatus != null ? src.AccountStatus.Name : string.Empty))

                .ForMember(dest => dest.RoleNames,
                    opt => opt.MapFrom(src =>
                        src.IDN_AccountRoles != null
                            ? src.IDN_AccountRoles
                                .Where(r => r.Role != null)
                                .Select(r => r.Role.RoleName)
                                .ToList()
                            : new List<string>()))

                .ForMember(dest => dest.StudentInfo,
                    opt => opt.MapFrom(src => src.IDN_StudentAccount))

                .ForMember(dest => dest.TeacherInfo,
                    opt => opt.MapFrom(src => src.IDN_TeacherAccount))
                .ReverseMap();

            // UpdateAccountRequest -> IDN_Account
            CreateMap<UpdateAccountRequest, IDN_Account>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateAccountRequest, IDN_Account>()
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
               .ForMember(dest => dest.CID, opt => opt.MapFrom(src => src.CID))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl));

            CreateMap<IDN_Student, StudentResponse>();

            CreateMap<IDN_Teacher, TeacherDetailResponse>();
            CreateMap<AccountResponse,LoginAccountResponse>()
                .ForMember(dest => dest.StudentInfo,
                    opt => opt.MapFrom(src => src.StudentInfo))
                .ForMember(dest => dest.TeacherInfo,
                    opt => opt.MapFrom(src => src.TeacherInfo))
                .ForMember(dest => dest.RoleNames,
                    opt => opt.MapFrom(src => src.RoleNames))
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FullName))
                .ReverseMap();
        }
    }
}
