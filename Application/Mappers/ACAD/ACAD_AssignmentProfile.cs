using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Assignment.Requests;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateAssignmentRequest, ACAD_Assignment>();
            CreateMap<UpdateAssignmentRequest, ACAD_Assignment>();
            // CreateMap<ACAD_Assignment, AssignmentResponse>();
            CreateMap<ACAD_Assignment, AssignmentResponse>()
             .ForMember(dest => dest.ClassMeetingId, opt => opt.MapFrom(src => src.ClassMeetingID ?? Guid.Empty))
             .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.CreatedBy))
             .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueAt ?? DateTime.MinValue));

            CreateMap<ACAD_Submission, SubmissionResponse>()
                .ForMember(dest => dest.Assignment, opt => opt.MapFrom(src => src.Assignment))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        }
    }
}
