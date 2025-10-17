using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Syllabus.Requests;
using DTOs.ACAD.ACAD_Syllabus.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Requests;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_SyllabusService
         : BaseService<ACAD_Syllabus, SyllabusResponse, UpdateSyllabusRequest, CreateSyllabusRequest>,
           IACAD_SyllabusService
    {
        private readonly IACAD_SyllabusRepository _syllabusRepo;
        private readonly IACAD_SyllabusItemRepository _syllabusItemRepo;

        public ACAD_SyllabusService(
            IACAD_SyllabusRepository syllabusRepo,
            IACAD_SyllabusItemRepository syllabusItemRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper
        ) : base(syllabusRepo, unitOfWork, mapper)
        {
            _syllabusRepo = syllabusRepo;
            _syllabusItemRepo = syllabusItemRepo;
        }

        public async Task<IEnumerable<SyllabusResponse>> GetSyllabiByCourseAsync(Guid courseId)
        {
            var syllabi = await _syllabusRepo.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<SyllabusResponse>>(syllabi);
        }

        public async Task<IEnumerable<SyllabusItemResponse>> GetItemsBySyllabusAsync(Guid syllabusId)
        {
            var items = await _syllabusItemRepo.GetBySyllabusIdAsync(syllabusId);
            return _mapper.Map<IEnumerable<SyllabusItemResponse>>(items);
        }


        public async Task<SyllabusItemResponse> AddSyllabusItemAsync(CreateSyllabusItemRequest request)
        {
            var entity = _mapper.Map<ACAD_SyllabusItem>(request);
            _syllabusItemRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SyllabusItemResponse>(entity);
        }

        public async Task<SyllabusItemResponse> UpdateSyllabusItemAsync(UpdateSyllabusItemRequest request)
        {
            var entity = await _syllabusItemRepo.GetByIdAsync(request.SyllabusItemID)
                         ?? throw new KeyNotFoundException("Syllabus item not found");

            _mapper.Map(request, entity); 
            _syllabusItemRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SyllabusItemResponse>(entity);
        }
        public async Task<SyllabusResponse> CreateSyllabusAsync(CreateSyllabusRequest request)
        {
            var entity = _mapper.Map<ACAD_Syllabus>(request);
            _syllabusRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SyllabusResponse>(entity);
        }

        public async Task<SyllabusResponse> SoftDeleteAsync(Guid id)
        {
            var entity = await _syllabusRepo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException("Syllabus not found");
            entity.IsDeleted = true;
            _syllabusRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SyllabusResponse>(entity);

        }

        public async Task<SyllabusItemResponse> SoftDeleteSyllabusItemAsync(Guid id)
        {
            var entity = await _syllabusItemRepo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException("Syllabus item not found");
            entity.IsDeleted = true;
            _syllabusItemRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SyllabusItemResponse>(entity);
        }
    }
}
