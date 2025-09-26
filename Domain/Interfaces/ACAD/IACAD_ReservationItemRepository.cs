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
    }
}
