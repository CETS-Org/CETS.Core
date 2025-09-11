using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseRequirement.Requests;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseRequirementService : IACAD_CourseRequirementService
    {
        private readonly IACAD_CourseRequirementRepository _courseRequirementRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseRequirementService(IACAD_CourseRequirementRepository courseRequirementRepo, IUnitOfWork uow, IMapper mapper)
        {
            _courseRequirementRepo = courseRequirementRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CourseRequirementResponse> CreateCourseRequirementAsync(CreateCourseRequirementRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var exists = await _courseRequirementRepo.ExistsAsync(request.CourseID, request.RequirementID);
                if (exists)
                    throw new InvalidOperationException("This requirement is already associated with the course");

                var entity = _mapper.Map<ACAD_CourseRequirement>(request);
                
                _courseRequirementRepo.Add(entity);
                await _uow.SaveChangesAsync();

                // Reload with includes to get proper navigation properties
                var createdEntity = await _courseRequirementRepo.GetByIdAsync(entity.Id);
                return _mapper.Map<CourseRequirementResponse>(createdEntity);
            });
        }

        public async Task<CourseRequirementResponse> UpdateCourseRequirementAsync(Guid id, UpdateCourseRequirementRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRequirementRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Course requirement not found");

                var exists = await _courseRequirementRepo.ExistsAsync(request.CourseID, request.RequirementID);
                if (exists && (entity.CourseID != request.CourseID || entity.RequirementID != request.RequirementID))
                    throw new InvalidOperationException("This requirement is already associated with the course");

                _mapper.Map(request, entity);
                _courseRequirementRepo.Update(entity);
                await _uow.SaveChangesAsync();

                // Reload with includes to get proper navigation properties
                var updatedEntity = await _courseRequirementRepo.GetByIdAsync(entity.Id);
                return _mapper.Map<CourseRequirementResponse>(updatedEntity);
            });
        }


        public async Task DeleteCourseRequirementAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseRequirementRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Course requirement not found");

                _courseRequirementRepo.Remove(entity);
            });
        }

        public async Task<CourseRequirementResponse?> GetCourseRequirementByIdAsync(Guid id)
        {
            var entity = await _courseRequirementRepo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CourseRequirementResponse>(entity);
        }

        public async Task<IEnumerable<CourseRequirementResponse>> GetRequirementsByCourseIdAsync(Guid courseId)
        {
            var entities = await _courseRequirementRepo.GetRequirementsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<CourseRequirementResponse>>(entities);
        }

        public async Task<IEnumerable<CourseRequirementResponse>> GetAllCourseRequirementsAsync()
        {
            var entities = await _courseRequirementRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseRequirementResponse>>(entities);
        }
    }
}
