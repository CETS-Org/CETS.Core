using DTOs.ACAD.ACAD_CourseWishlist.Requests;
using DTOs.ACAD.ACAD_CourseWishlist.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseWishlistService
    {
        Task<WishlistItemResponse> AddCourseToWishlistAsync(AddToWishlistRequest request);
        Task<bool> IsCourseInWishlistAsync(Guid studentId, Guid courseId);
        Task<IEnumerable<WishlistItemResponse>> GetStudentWishlistAsync(Guid studentId);
        Task RemoveCourseFromWishlistAsync(Guid studentId, Guid courseId);
    }
}

