using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseService
    {
        Task<Guid> CreateCourseAsync(CreateCourseRequest request);
        Task UpdateCourseAsync(UpdateCourseRequest request);
        Task DeleteCourseAsync(Guid id);
        Task<IEnumerable<CourseDetailResponse>> GetAllCoursesAsync();
        Task<CourseResponse?> GetCourseByIdAsync(Guid id);

        Task<IEnumerable<CourseResponse>> SearchCoursesAsync(string keyword);
        Task<IEnumerable<CourseResponse>> FilterCoursesAsync(FilterCourseRequest request);
        Task<CourseDetailResponse?> GetCourseDetailAsync(Guid courseId);
        Task<IReadOnlyList<CourseListItemResponse>> GetAllCoursesForListAsync();
    }

}
