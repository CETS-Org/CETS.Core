using Application.Interfaces.ACAD;
using Application.Interfaces.IDN;
using Application.Interfaces.COM;
using Application.Interfaces.Common.Storage;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ACAD.ACAD_Course.Search;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseService : IACAD_CourseService
    {
        private readonly IACAD_CourseRepository _courseRepo;
        private readonly IACAD_CourseBenefitRepository _courseBenefitRepo;
        private readonly IACAD_CourseRequirementRepository _courseRequirementRepo;
        private readonly IACAD_CourseSkillRepository _courseSkillRepo;
        private readonly IACAD_CourseScheduleRepository _courseScheduleRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public ACAD_CourseService(
            IACAD_CourseRepository courseRepo,
            IACAD_CourseBenefitRepository courseBenefitRepo,
            IACAD_CourseRequirementRepository courseRequirementRepo,
            IACAD_CourseSkillRepository courseSkillRepo,
            IACAD_CourseScheduleRepository courseScheduleRepo,
            IUnitOfWork uow, 
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _courseRepo = courseRepo;
            _courseBenefitRepo = courseBenefitRepo;
            _courseRequirementRepo = courseRequirementRepo;
            _courseSkillRepo = courseSkillRepo;
            _courseScheduleRepo = courseScheduleRepo;
            _uow = uow;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
         
        }

        public async Task<IReadOnlyList<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CourseResponse>>(courses);
        }

        public async Task<Guid> CreateCourseAsync(CreateCourseRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // Set default image if not provided
                if (string.IsNullOrEmpty(request.CourseImageUrl))
                {
                    request.CourseImageUrl = "https://pub-59cfd11e5f0d4b00af54839edc83842d.r2.dev/images/course-tmp-image.jpg";
                }

                // Create the main course entity
                var entity = _mapper.Map<ACAD_Course>(request);
                entity.IsActive = false; 

                _courseRepo.Add(entity);
                await _uow.SaveChangesAsync(); // Save to generate the course ID

                // Create course benefits
                if (request.BenefitIDs != null && request.BenefitIDs.Any())
                {
                    foreach (var benefitId in request.BenefitIDs)
                    {
                        var courseBenefit = new ACAD_CourseBenefit
                        {
                            CourseID = entity.Id,
                            BenefitID = benefitId
                        };
                        _courseBenefitRepo.Add(courseBenefit);
                    }
                }

                // Create course requirements
                if (request.RequirementIDs != null && request.RequirementIDs.Any())
                {
                    foreach (var requirementId in request.RequirementIDs)
                    {
                        var courseRequirement = new ACAD_CourseRequirement
                        {
                            CourseID = entity.Id,
                            RequirementID = requirementId
                        };
                        _courseRequirementRepo.Add(courseRequirement);
                    }
                }

                // Create course skills
                if (request.SkillIDs != null && request.SkillIDs.Any())
                {
                    foreach (var skillId in request.SkillIDs)
                    {
                        var courseSkill = new ACAD_CourseSkill
                        {
                            CourseID = entity.Id,
                            SkillID = skillId
                        };
                        _courseSkillRepo.Add(courseSkill);
                    }
                }

                // Create course schedules
                if (request.Schedules != null && request.Schedules.Any())
                {
                    foreach (var schedule in request.Schedules)
                    {
                        var courseSchedule = new ACAD_CourseSchedule
                        {
                            CourseID = entity.Id,
                            TimeSlotID = schedule.TimeSlotID,
                            DayOfWeek = schedule.DayOfWeek
                        };
                        _courseScheduleRepo.Add(courseSchedule);
                    }
                }

                await _uow.SaveChangesAsync(); // Save all related entities

                return entity.Id;
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

        public async Task SoftDeleteCourseAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Course not found");

                entity.IsDeleted = true;
                _courseRepo.Update(entity);
            });
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
        public async Task<IReadOnlyList<CourseDetailResponse>> GetAllCoursesDetailsAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CourseDetailResponse>>(courses);
        }

        public async Task<IReadOnlyList<CourseListItemResponse>> GetAllCoursesForListAsync()
        {
            var courses = await _courseRepo.GetAllCoursesForListAsync().ToListAsync();
            return _mapper.Map<IReadOnlyList<CourseListItemResponse>>(courses);
        }

        public async Task<CourseSearchResult> SearchBasicAsync(CourseSearchQuery q, CancellationToken ct)
        {
            return await _courseRepo.SearchBasicAsync(q, ct);
        }

        public async Task ActivateCourseAsync(Guid courseId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRepo.GetByIdAsync(courseId);
                if (entity == null)
                    throw new KeyNotFoundException("Course not found");

                entity.IsActive = true;
                entity.UpdatedAt = DateTime.UtcNow;

                _courseRepo.Update(entity);
            });
        }

        public async Task DeactivateCourseAsync(Guid courseId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRepo.GetByIdAsync(courseId);
                if (entity == null)
                    throw new KeyNotFoundException("Course not found");

                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;

                _courseRepo.Update(entity);
            });
        }

        public async Task<CourseImageUploadResponse> GetImageUploadUrlAsync(string fileName, string contentType)
        {
            // Get presigned upload URL and generated file path
            var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("courses", fileName, contentType);
            var publicUrl = _fileStorageService.GetPublicUrl(filePath);

            return new CourseImageUploadResponse
            {
                UploadUrl = uploadUrl,
                FilePath = filePath,
                PublicUrl = publicUrl
            };
        }
    }

}
