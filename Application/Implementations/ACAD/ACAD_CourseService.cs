using Application.Interfaces.ACAD;
using Application.Interfaces.IDN;
using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
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

        public ACAD_CourseService(
            IACAD_CourseRepository courseRepo, 
            IUnitOfWork uow, 
            IMapper mapper)
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
                entity.IsActive = true;

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
            return _mapper.Map<CourseDetailResponse?>(entity);
        }

        public async Task<IReadOnlyList<CourseListItemResponse>> GetAllCoursesForListAsync()
        {
            var courses = await _courseRepo.GetAllCoursesForListAsync().ToListAsync();
            return _mapper.Map<IReadOnlyList<CourseListItemResponse>>(courses);
        }
    }

}
