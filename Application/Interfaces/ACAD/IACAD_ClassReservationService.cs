using Domain.Entities;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_Course.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_ClassReservationService
    {
        IQueryable<ClassReservationResponse> GetAllAsQueryable();
        Task<IReadOnlyList<ClassReservationResponse>> GetAllReservationAsync();

        Task<ClassReservationResponse?> GetReservationByStudentId(Guid id);

        Task<ClassReservationResponse?> GetReservationById(Guid id);

        Task<Guid> CreateReservationAsync(CreateClassReservationRequest request);

        Task<ClassReservationResponse?> UpdateReservationAsync(UpdateClassReservationRequest request);


    }
}
