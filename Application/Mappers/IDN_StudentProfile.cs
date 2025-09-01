using AutoMapper;
using Domain.Entities;
using DTOs.IDN_Student.Requests;
using DTOs.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class IDN_StudentProfile : Profile
    {
        public IDN_StudentProfile() 
        {
            CreateMap<IDN_Student, StudentResponse>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<CreateStudentRequest, IDN_Student>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId));

            CreateMap<IDN_Student, UpdateStudentRequest>()
                .ReverseMap();
        }
    }
}
