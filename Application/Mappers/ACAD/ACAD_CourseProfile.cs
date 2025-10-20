using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseBenefit.Responses;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;
using DTOs.ACAD.ACAD_CourseSkill.Responses;
using DTOs.ACAD.ACAD_Syllabus.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseRequest, ACAD_Course>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective))
                .ForMember(dest => dest.ACAD_CourseBenefits, opt => opt.MapFrom(src => 
                    src.BenefitIDs != null ? src.BenefitIDs.Select(id => new ACAD_CourseBenefit { BenefitID = id }) : new List<ACAD_CourseBenefit>()))
                .ForMember(dest => dest.ACAD_CourseRequirements, opt => opt.MapFrom(src => 
                    src.RequirementIDs != null ? src.RequirementIDs.Select(id => new ACAD_CourseRequirement { RequirementID = id }) : new List<ACAD_CourseRequirement>()))
                .ForMember(dest => dest.ACAD_CourseSkills, opt => opt.MapFrom(src => 
                    src.SkillIDs != null ? src.SkillIDs.Select(id => new ACAD_CourseSkill { SkillID = id }) : new List<ACAD_CourseSkill>()))
                .ForMember(dest => dest.ACAD_CourseSchedules, opt => opt.MapFrom(src => 
                    src.Schedules != null ? src.Schedules.Select(s => new ACAD_CourseSchedule { TimeSlotID = s.TimeSlotID, DayOfWeek = s.DayOfWeek }) : new List<ACAD_CourseSchedule>()))
                .ForMember(dest => dest.ACAD_Syllabi, opt => opt.MapFrom(src => src.Syllabi ?? new List<CreateCourseSyllabusDetail>()));
            
            CreateMap<CreateCourseScheduleDetail, ACAD_CourseSchedule>();
            
            CreateMap<CreateCourseSyllabusDetail, ACAD_Syllabus>()
                .ForMember(dest => dest.ACAD_SyllabusItems, opt => opt.MapFrom(src => src.Items ?? new List<CreateCourseSyllabusItemDetail>()));
            
            CreateMap<CreateCourseSyllabusItemDetail, ACAD_SyllabusItem>();
            
            CreateMap<UpdateCourseRequest, ACAD_Course>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));

            CreateMap<ACAD_Course, CourseResponse>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));

            CreateMap<ACAD_Course, CourseDetailResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.CourseLevel != null ? src.CourseLevel.Name : ""))
                .ForMember(dest => dest.FormatName, opt => opt.MapFrom(src => src.CourseFormat != null ? src.CourseFormat.Name : ""))
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective))
                
                // Teacher information
                .ForMember(dest => dest.TeacherDetails, opt => opt.MapFrom(src => 
                    src.ACAD_CourseTeacherAssignments.Select(a => a.Teacher).ToList()))
                
                // Course statistics
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => 
                    src.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).Sum(i => i.TotalSlots ?? 0) + " slots"))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                    (double)(src.AverageRating ?? 0)))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => 
                    src.ACAD_Enrollments.Count(e => !e.IsDeleted)))
                
                // Audit information
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 
                    src.CreatedByNavigation != null ? src.CreatedByNavigation.Email : null))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => 
                    src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Email : null))
                
                // Detailed course content
                .ForMember(dest => dest.Syllabi, opt => opt.MapFrom(src => 
                    src.ACAD_Syllabi.Where(s => !s.IsDeleted).ToList()))
                .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src => src.ACAD_CourseBenefits))
                .ForMember(dest => dest.Requirements, opt => opt.MapFrom(src => src.ACAD_CourseRequirements))
                .ForMember(dest => dest.CourseSkills, opt => opt.MapFrom(src => src.ACAD_CourseSkills));

            CreateMap<ACAD_Course, CourseListItemResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TeacherDetails, opt => opt.MapFrom(src => 
                    src.ACAD_CourseTeacherAssignments.Select(a => a.Teacher).ToList()))
                 .ForMember(dest => dest.CourseSkills, opt => opt.MapFrom(src =>
                    src.ACAD_CourseSkills))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => 
                    src.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).Sum(i => i.TotalSlots ?? 0) + " slots"))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.CourseLevel.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                    (double)(src.AverageRating ?? 0)))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => 
                    src.ACAD_Enrollments.Count(e => !e.IsDeleted)))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.ACAD_CourseSchedules))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));


            // Nested object mappings
            CreateMap<IDN_Teacher, TeacherAcademicDetailResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.TotalStudents, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCourses, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Account.AvatarUrl));


            CreateMap<ACAD_SyllabusItem, SyllabusItemResponse>();

            CreateMap<ACAD_Syllabus, SyllabusResponse>()
                .ForMember(dest => dest.SyllabusID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => 
                    src.ACAD_SyllabusItems.Where(i => !i.IsDeleted).OrderBy(i => i.SessionNumber).ToList()));

            CreateMap<ACAD_CourseBenefit, CourseBenefitResponse>()
                .ForMember(dest => dest.BenefitName, opt => opt.MapFrom(src => src.Benefit.Name));

            CreateMap<ACAD_CourseRequirement, CourseRequirementResponse>()
                .ForMember(dest => dest.RequirementName, opt => opt.MapFrom(src => src.Requirement.Name));

            CreateMap<ACAD_CourseSkill, CourseSkillResponse>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));
            CreateMap<ACAD_Course, TeachingCourseResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.CourseLevel.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseCode))
                .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.CourseImageUrl))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.FormatName, opt => opt.MapFrom(src => src.CourseFormat.Name));


        }
    }
}
