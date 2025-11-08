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
            CreateMap<CreateAssignmentRequest, ACAD_Assignment>()
                .ForMember(dest => dest.ClassMeetingID, opt => opt.MapFrom(src => src.ClassMeetingId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.AssignmentType, opt => opt.MapFrom(src => src.AssignmentType))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateAssignmentWithFileRequest, ACAD_Assignment>()
                .ForMember(dest => dest.ClassMeetingID, opt => opt.MapFrom(src => src.ClassMeetingId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.AssignmentType, opt => opt.MapFrom(src => src.AssignmentType))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateQuizAssignmentRequest, ACAD_Assignment>()
                .ForMember(dest => dest.ClassMeetingID, opt => opt.MapFrom(src => src.ClassMeetingId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.AssignmentType, opt => opt.MapFrom(src => src.AssignmentType))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateSpeakingAssignmentRequest, ACAD_Assignment>()
                .ForMember(dest => dest.ClassMeetingID, opt => opt.MapFrom(src => src.ClassMeetingId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.AssignmentType, opt => opt.MapFrom(src => src.AssignmentType))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateAssignmentRequest, ACAD_Assignment>();

            // map Submission
            CreateMap<ACAD_Submission, SubmissionResponse>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.StudentID))
             .ForMember(dest => dest.StoreUrl, opt => opt.MapFrom(src => src.StoreUrl))
             .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback))
             .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
             .ForMember(dest => dest.IsAiScore, opt => opt.MapFrom(src => src.IsAiScore))
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // map Assignment
            CreateMap<ACAD_Assignment, AssignmentResponse>()
             .ForMember(dest => dest.ClassMeetingId, opt => opt.MapFrom(src => src.ClassMeetingID ?? Guid.Empty))
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.StoreUrl))
             .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueAt ?? DateTime.Now))
             .ForMember(dest => dest.Submissions, opt => opt.MapFrom(src => src.ACAD_Submissions))
             .ForMember(dest => dest.SkillID, opt => opt.MapFrom(src => src.SkillID))
             .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : null))
             .ForMember(dest => dest.AssignmentType, opt => opt.MapFrom(src => src.AssignmentType ?? "homework"))
             .ForMember(dest => dest.QuestionUrl, opt => opt.Ignore());

            CreateMap<ACAD_Assignment, AssignmentUploadResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title ?? string.Empty))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueAt ?? DateTime.Now))
                .ForMember(dest => dest.UploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore());

            CreateMap<ACAD_Assignment, QuizAssignmentResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassMeetingId, opt => opt.MapFrom(src => src.ClassMeetingID ?? Guid.Empty))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueAt ?? DateTime.Now))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.SkillID, opt => opt.MapFrom(src => src.SkillID))
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : null))
                .ForMember(dest => dest.UploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionJson, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionJsonUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionFilePath, opt => opt.Ignore());

            CreateMap<ACAD_Assignment, SpeakingAssignmentResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassMeetingId, opt => opt.MapFrom(src => src.ClassMeetingID ?? Guid.Empty))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueAt ?? DateTime.Now))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.SkillID, opt => opt.MapFrom(src => src.SkillID))
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : null))
                .ForMember(dest => dest.UploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionJson, opt => opt.Ignore())
                .ForMember(dest => dest.AudioUploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.VideoUploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionJsonUrl, opt => opt.Ignore());
        }
    }
}
