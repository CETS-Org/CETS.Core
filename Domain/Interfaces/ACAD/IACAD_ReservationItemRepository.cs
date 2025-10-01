using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ReservationItemRepository : IBaseRepository<ACAD_ReservationItem>
    {
        Task<ACAD_ReservationItem?> GetByReservationIdAsync(Guid reservationId);
        Task<ACAD_ReservationItem?> GetReservationItemByIdAsync(Guid reservationItemId);
        IQueryable<ACAD_ReservationItem?> GetAllReservationItem();
        IQueryable<ACAD_ReservationItem?> GetReservationItemByReservationId(Guid id);
        Task<bool> ExistsByReservationAndCourseAsync(Guid reservationId, Guid courseId);
        Task<List<Guid>> GetActiveReservationCoursesForStudentAsync(Guid studentId, DateTime currentTime);
    }
}
