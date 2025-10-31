using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_SubmissionProfile : Profile
    {
        public ACAD_SubmissionProfile()
        {
            CreateMap<SubmitAssignmentRequest, ACAD_Submission>();

            CreateMap<ACAD_Submission, SubmissionResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student.StudentCode))
                .ForMember(dest => dest.StoreUrl, opt => opt.MapFrom(src => src.StoreUrl));

            CreateMap<ACAD_Submission, SubmitAssignmentRequest>()
                .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.AssignmentID ?? Guid.Empty))
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.StudentID));

            CreateMap<ACAD_Submission, SubmissionDetailResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment.Title));
        }
    }
}
