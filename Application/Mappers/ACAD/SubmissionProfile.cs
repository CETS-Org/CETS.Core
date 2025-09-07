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
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<SubmitAssignmentRequest, ACAD_Submission>();

            CreateMap<ACAD_Submission, SubmissionResponse>();
            CreateMap<ACAD_Submission, SubmissionDetailResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment.Title));
        }
    }
}
