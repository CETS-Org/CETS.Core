using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseBenefit.Requests;
using DTOs.ACAD.ACAD_CourseBenefit.Responses;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseBenefitService : IACAD_CourseBenefitService
    {
        private readonly IACAD_CourseBenefitRepository _courseBenefitRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseBenefitService(IACAD_CourseBenefitRepository courseBenefitRepo, IUnitOfWork uow, IMapper mapper)
        {
            _courseBenefitRepo = courseBenefitRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CourseBenefitResponse> CreateCourseBenefitAsync(CreateCourseBenefitRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var exists = await _courseBenefitRepo.ExistsAsync(request.CourseID, request.BenefitID);
                if (exists)
                    throw new InvalidOperationException("This benefit is already associated with the course");

                var entity = _mapper.Map<ACAD_CourseBenefit>(request);

                _courseBenefitRepo.Add(entity);
                await _uow.SaveChangesAsync();

                // Reload with includes to get proper navigation properties
                var createdEntity = await _courseBenefitRepo.GetByIdAsync(entity.Id);
                return _mapper.Map<CourseBenefitResponse>(createdEntity);
            });
        }


        public async Task<CourseBenefitResponse> UpdateCourseBenefitAsync(Guid id, UpdateCourseBenefitRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseBenefitRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Course benefit not found");

                var exists = await _courseBenefitRepo.ExistsAsync(request.CourseID, request.BenefitID);
                if (exists && (entity.CourseID != request.CourseID || entity.BenefitID != request.BenefitID))
                    throw new InvalidOperationException("This benefit is already associated with the course");

                _mapper.Map(request, entity);
                _courseBenefitRepo.Update(entity);
                await _uow.SaveChangesAsync();

                // Reload with includes to get proper navigation properties
                var updatedEntity = await _courseBenefitRepo.GetByIdAsync(entity.Id);
                return _mapper.Map<CourseBenefitResponse>(updatedEntity);
            });
        }


        public async Task DeleteCourseBenefitAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseBenefitRepo.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException("Course benefit not found");

                _courseBenefitRepo.Remove(entity);
            });
        }

        public async Task<CourseBenefitResponse?> GetCourseBenefitByIdAsync(Guid id)
        {
            var entity = await _courseBenefitRepo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CourseBenefitResponse>(entity);
        }

        public async Task<IEnumerable<CourseBenefitResponse>> GetBenefitsByCourseIdAsync(Guid courseId)
        {
            var entities = await _courseBenefitRepo.GetBenefitsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<CourseBenefitResponse>>(entities);
        }

        public async Task<IEnumerable<CourseBenefitResponse>> GetAllCourseBenefitsAsync()
        {
            var entities = await _courseBenefitRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseBenefitResponse>>(entities);
        }
    }
}
