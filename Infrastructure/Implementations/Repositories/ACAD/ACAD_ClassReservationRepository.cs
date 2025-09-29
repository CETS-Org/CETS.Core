using AutoMapper;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassReservationRepository : BaseRepository<ACAD_ClassReservation>, IACAD_ClassReservationRepository
    {
        private readonly IMapper _mapper;
        public ACAD_ClassReservationRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public IQueryable<ACAD_ClassReservation> GetAllReservation()
        {
            return _context.ACAD_ClassReservations
                .AsNoTracking()
                .Include(cr => cr.Student)
                .Include(cr => cr.CoursePackage)
                .Include(cr => cr.ReservationStatus)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.Course)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.Invoice)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.PlanType)
                .AsQueryable()
                .OrderByDescending(cr => cr.Id);
        }
        public IQueryable<ACAD_ClassReservation> GetReservationByStudentId(Guid studentId)
        {
            return _context.ACAD_ClassReservations
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .Where(cr => cr.StudentID == studentId)
                .Include(cr => cr.Student)
                .Include(cr => cr.CoursePackage)
                .Include(cr => cr.ReservationStatus)
                .Include(cr => cr.ACAD_ReservationItems).ThenInclude(ri => ri.Course)
                .Include(cr => cr.ACAD_ReservationItems).ThenInclude(ri => ri.Invoice)
                .Include(cr => cr.ACAD_ReservationItems).ThenInclude(ri => ri.PlanType)
                .AsQueryable()
                .OrderByDescending(cr => cr.Id);
                
        }
        public async Task<ACAD_ClassReservation?> GetReservationById(Guid id)
        {
            return await _context.ACAD_ClassReservations
                .AsNoTracking()
                .Include(cr => cr.Student)
                .Include(cr => cr.CoursePackage)
                .Include(cr => cr.ReservationStatus)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.Course)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.Invoice)
                .Include(cr => cr.ACAD_ReservationItems)
                .ThenInclude(ri => ri.PlanType)
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }


    }
}


