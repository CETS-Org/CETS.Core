using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassReservationRepository : IBaseRepository<ACAD_ClassReservation>
    {
        IQueryable<ACAD_ClassReservation> GetAllReservation();
        IQueryable<ACAD_ClassReservation> GetReservationByStudentId(Guid id);
        Task<ACAD_ClassReservation?> GetReservationById(Guid id);

    }


}


