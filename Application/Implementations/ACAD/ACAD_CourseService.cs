using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseService : IACAD_CourseService
    {
        private readonly IACAD_CourseRepository _courseRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseService(IACAD_CourseRepository courseRepo, IUnitOfWork uow, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> CreateCourseAsync(CreateCourseRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(() =>
            {
                var entity = _mapper.Map<ACAD_Course>(request);
                entity.Id = Guid.NewGuid();
                entity.IsActive = true;
                entity.CreatedAt = DateTime.UtcNow;

                _courseRepo.Add(entity);
                return Task.FromResult(entity.Id);
            });
        }

        public async Task UpdateCourseAsync(UpdateCourseRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException("Course not found");

                _mapper.Map(request, entity);
                entity.UpdatedAt = DateTime.UtcNow;

                _courseRepo.Update(entity);
            });
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(() =>
                _courseRepo.RemoveByIdAsync(id)
            );
        }

        public async Task<IEnumerable<CourseDetailResponse>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDetailResponse>>(courses);
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            return _mapper.Map<CourseResponse?>(course);
        }

        public async Task<IEnumerable<CourseResponse>> SearchCoursesAsync(string keyword)
        {
            var result = await _courseRepo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<CourseResponse>>(result);
        }

        public async Task<IEnumerable<CourseResponse>> FilterCoursesAsync(FilterCourseRequest request)
        {
            var result = await _courseRepo.FilterAsync(request.LevelId, request.FormatId, request.TeacherId);
            return _mapper.Map<IEnumerable<CourseResponse>>(result);
        }

        public async Task<CourseDetailResponse?> GetCourseDetailAsync(Guid courseId)
        {
            var entity = await _courseRepo.GetDetailAsync(courseId);
            if (entity == null) return null;

            // Create detailed response with all extra data
            var response = new CourseDetailResponse
            {
                Id = entity.Id,
                CourseCode = entity.CourseCode,
                CourseName = entity.CourseName,
                Description = entity.Description,
                Price = entity.StandardPrice,
                CategoryName = entity.Category?.Name ?? "",
                LevelName = entity.CourseLevel?.Name ?? "",
                FormatName = entity.CourseFormat?.Name ?? "",
                
                // Extra data like in list view
                Teacher = entity.ACAD_CourseTeacherAssignments
                    .Select(a => a.Teacher.Account.FullName)
                    .FirstOrDefault() ?? "TBA",
                Duration = (entity.ACAD_Syllabi
                    .SelectMany(s => s.ACAD_SyllabusItems)
                    .Sum(i => i.EstimatedMinutes ?? 0) / 60.0).ToString("0.0") + " hours",
                Rating = entity.COM_Feedbacks.Any() ? entity.COM_Feedbacks.Average(f => (double?)f.Rating) ?? 0.0 : 0.0,
                StudentsCount = entity.ACAD_Enrollments.Count(e => !e.IsDeleted),
                Image = entity.CourseImageUrl ?? "",
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                
                // Additional detail fields
                Teachers = entity.ACAD_CourseTeacherAssignments
                    .Select(a => a.Teacher.Account.FullName)
                    .ToList(),
                SyllabusItems = entity.ACAD_Syllabi
                    .SelectMany(s => s.ACAD_SyllabusItems)
                    .OrderBy(i => i.SessionNumber)
                    .Select(i => new SyllabusItemResponse
                    {
                        SessionNumber = i.SessionNumber,
                        TopicTitle = i.TopicTitle,
                        EstimatedMinutes = i.EstimatedMinutes,
                        Required = i.Required,
                        Objectives = i.Objectives,
                        ContentSummary = i.ContentSummary
                    })
                    .ToList()
            };

            return response;
        }

        public async Task<IReadOnlyList<CourseListItemResponse>> GetAllCoursesForListAsync()
        {
            var coursesQuery = _courseRepo.GetAllCoursesForListAsync();

            var courses = await coursesQuery.ToListAsync();
            
            var courseDtos = courses.Select(c => new CourseListItemResponse
            {
                Id = c.Id.ToString(),
                CourseName = c.CourseName,
                Description = c.Description ?? "",
                Teacher = c.ACAD_CourseTeacherAssignments
                    .Select(a => a.Teacher.Account.FullName)
                    .FirstOrDefault() ?? "TBA",
                Duration = (c.ACAD_Syllabi
                    .SelectMany(s => s.ACAD_SyllabusItems)
                    .Sum(i => i.EstimatedMinutes ?? 0) / 60.0).ToString("0.0") + " hours",
                Level = c.CourseLevel.Name,
                Price = c.StandardPrice,
                Rating = c.COM_Feedbacks.Any() ? c.COM_Feedbacks.Average(f => (double?)f.Rating) ?? 0.0 : 0.0,
                StudentsCount = c.ACAD_Enrollments.Count(e => !e.IsDeleted),
                Image = c.CourseImageUrl ?? "",
                Category = c.Category.Name
            })
            .OrderBy(c => c.CourseName)
            .ToList();

            return courseDtos;
        }
    }

}
