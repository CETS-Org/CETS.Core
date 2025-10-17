using Domain.Entities;
using DTOs.ACAD.ACAD_Syllabus.Requests;
using DTOs.ACAD.ACAD_Syllabus.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Requests;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_SyllabusService : IBaseService<ACAD_Syllabus, SyllabusResponse, UpdateSyllabusRequest, CreateSyllabusRequest>
    {
        Task<SyllabusResponse> CreateSyllabusAsync(CreateSyllabusRequest request);
        Task<SyllabusItemResponse> AddSyllabusItemAsync(CreateSyllabusItemRequest request);

        Task<IEnumerable<SyllabusResponse>> GetSyllabiByCourseAsync(Guid courseId);
        Task<IEnumerable<SyllabusItemResponse>> GetItemsBySyllabusAsync(Guid syllabusId);

        Task<SyllabusItemResponse> UpdateSyllabusItemAsync(UpdateSyllabusItemRequest request);
    }

}
