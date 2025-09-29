using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ReservationItemRepository : BaseRepository<ACAD_ReservationItem>, IACAD_ReservationItemRepository
    {
        public ACAD_ReservationItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<ACAD_ReservationItem?> GetByReservationIdAsync(Guid reservationItemId)
        {
            return await _context.ACAD_ReservationItems
                .Include(ri => ri.Course)
                .Include(ri => ri.PlanType)
                .FirstOrDefaultAsync(ri => ri.Id == reservationItemId);
        }
    }
}
