using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseSkill.Requests;
using DTOs.ACAD.ACAD_CourseSkill.Responses;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseSkillService : IACAD_CourseSkillService
    {
        private readonly IACAD_CourseSkillRepository _courseSkillRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseSkillService(
            IACAD_CourseSkillRepository courseSkillRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _courseSkillRepo = courseSkillRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Guid> CreateCourseSkillAsync(CreateSkillRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var existing = await _courseSkillRepo.GetByCourseAndSkillAsync(request.CourseID, request.SkillID);
                if (existing != null)
                {
                    throw new InvalidOperationException("Course-Skill relationship already exists.");
                }

                var entity = _mapper.Map<ACAD_CourseSkill>(request);

                _courseSkillRepo.Add(entity);
                await _uow.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task UpdateCourseSkillAsync(Guid id, UpdateCourseSkillRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseSkillRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"CourseSkill with ID {id} not found.");
                }

                var existing = await _courseSkillRepo.GetByCourseAndSkillAsync(request.CourseID, request.SkillID);
                if (existing != null && existing.Id != id)
                {
                    throw new InvalidOperationException("Course-Skill relationship already exists.");
                }

                _mapper.Map(request, entity);

                _courseSkillRepo.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task DeleteCourseSkillAsync(Guid id)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _courseSkillRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"CourseSkill with ID {id} not found.");
                }

                _courseSkillRepo.Remove(entity);
                await _uow.SaveChangesAsync();
            });
        }

        public async Task<CourseSkillResponse?> GetCourseSkillByIdAsync(Guid id)
        {
            var entity = await _courseSkillRepo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CourseSkillResponse>(entity);
        }

        public async Task<IEnumerable<CourseSkillResponse>> GetAllCourseSkillsAsync()
        {
            var entities = await _courseSkillRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseSkillResponse>>(entities);
        }

        public async Task<IEnumerable<CourseSkillResponse>> GetCourseSkillsByCourseAsync(Guid courseId)
        {
            var entities = await _courseSkillRepo.GetByCourseAsync(courseId);
            return _mapper.Map<IEnumerable<CourseSkillResponse>>(entities);
        }

        public async Task<IEnumerable<CourseSkillResponse>> GetCourseSkillsBySkillAsync(Guid skillId)
        {
            var entities = await _courseSkillRepo.GetBySkillAsync(skillId);
            return _mapper.Map<IEnumerable<CourseSkillResponse>>(entities);
        }

        public async Task<CourseSkillResponse?> GetCourseSkillByCourseAndSkillAsync(Guid courseId, Guid skillId)
        {
            var entity = await _courseSkillRepo.GetByCourseAndSkillAsync(courseId, skillId);
            return entity == null ? null : _mapper.Map<CourseSkillResponse>(entity);
        }
    }
}
