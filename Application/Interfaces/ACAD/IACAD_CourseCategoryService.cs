using DTOs.ACAD.ACAD_CourseCategory.Request;
using DTOs.ACAD.ACAD_CourseCategory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseCategoryService
    {
        Task<Guid> CreateCourseCategoryAsync(CreateCourseCategoryRequest request);
        Task UpdateCourseCategoryAsync(UpdateCourseCategoryRequest request);
        Task DeleteCourseCategoryAsync(Guid id);
        Task<IEnumerable<CourseCategoryResponse>> GetAllCourseCategoriesAsync();
        Task<CourseCategoryResponse?> GetCourseCategoryByIdAsync(Guid id);
        Task<CourseCategoryResponse?> GetCourseCategoryByCodeAsync(string code);
    }
}
