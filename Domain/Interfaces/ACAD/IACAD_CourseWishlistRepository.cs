using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseWishlistRepository : IBaseRepository<ACAD_CourseWishlist>
    {
        Task<IEnumerable<ACAD_CourseWishlist>> GetWishlistByStudentIdAsync(Guid studentId);
        Task<bool> IsCourseInWishlistAsync(Guid studentId, Guid courseId);
        Task<ACAD_CourseWishlist?> GetWishlistItemAsync(Guid studentId, Guid courseId);
    }
}

