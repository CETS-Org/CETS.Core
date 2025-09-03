using Application.Interfaces.ACAD;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
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

        public ACAD_CourseService(IACAD_CourseRepository courseRepo, IUnitOfWork uow)
        {
            _courseRepo = courseRepo;
            _uow = uow;
        }
        public async Task<Guid> CreateCourseAsync(CreateCourseRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var course = new ACAD_Course
                {
                    Id = Guid.NewGuid(),
                    CourseCode = request.CourseCode,
                    CourseName = request.CourseName,
                    CourseLevelID = request.CourseLevelID,
                    CourseFormatID = request.CourseFormatID,
                    CategoryID = request.CategoryID,
                    StandardPrice = request.StandardPrice,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _courseRepo.Add(course);
                return course.Id;
            });
        }

        public async Task UpdateCourseAsync(UpdateCourseRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var course = await _courseRepo.GetByIdAsync(request.Id);
                if (course == null) throw new Exception("Course not found");

                course.CourseCode = request.CourseCode;
                course.CourseName = request.CourseName;
                course.CourseLevelID = request.CourseLevelID;
                course.CourseFormatID = request.CourseFormatID;
                course.CategoryID = request.CategoryID;
                course.StandardPrice = request.StandardPrice;
                course.UpdatedAt = DateTime.UtcNow;

                _courseRepo.Update(course);
            });
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                await _courseRepo.RemoveByIdAsync(id);
            });
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return courses.Select(c => new CourseResponse
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName
            });
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid id)
        {
            var c = await _courseRepo.GetByIdAsync(id);
            return c == null ? null : new CourseResponse
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName
            };
        }
        public async Task<IEnumerable<CourseResponse>> SearchCoursesAsync(string keyword)
        {
            var result = await _courseRepo.SearchAsync(keyword);
            return result.Select(c => new CourseResponse
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName
            });
        }

        public async Task<IEnumerable<CourseResponse>> FilterCoursesAsync(FilterCourseRequest request)
        {
            var result = await _courseRepo.FilterAsync(request.LevelId, request.FormatId, request.TeacherId);
            return result.Select(c => new CourseResponse
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName
            });
        }

        public async Task<CourseDetailResponse?> GetCourseDetailAsync(Guid courseId)
        {
            var c = await _courseRepo.GetDetailAsync(courseId);
            return c == null ? null : new CourseDetailResponse
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                StandardPrice = c.StandardPrice,
                CategoryName = c.Category?.Name ?? "",
                LevelName = c.CourseLevelID.ToString(),
                FormatName = c.CourseFormatID.ToString()
            };
        }
    }

}
