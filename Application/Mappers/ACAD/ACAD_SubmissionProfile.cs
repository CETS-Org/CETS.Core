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

            CreateMap<StartAttemptRequest, ACAD_Submission>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.Ignore())
                .ForMember(dest => dest.Feedback, opt => opt.Ignore())
                .ForMember(dest => dest.IsAiScore, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Assignment, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByNavigation, opt => opt.Ignore());

            CreateMap<ACAD_Submission, SubmissionResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student.StudentCode))
                .ForMember(dest => dest.StoreUrl, opt => opt.MapFrom(src => src.StoreUrl))
                .ForMember(dest => dest.IsAiScore, opt => opt.MapFrom(src => src.IsAiScore));

            CreateMap<ACAD_Submission, SubmitAssignmentRequest>()
                .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.AssignmentID ?? Guid.Empty))
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.StudentID));

            CreateMap<ACAD_Submission, SubmissionDetailResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.FullName))
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment.Title));
        }
    }
}
