using Domain.Entities;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_ReservationItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_ReservationItemService
    {
        
        IQueryable<ReservationItemResponse?> GetAllReservationItemAsync();
        Task<ReservationItemResponse?> GetReservationItemByIdAsync(Guid reservationItemId);
        Task<ReservationItemResponse?> GetReservationItemByReservationId(Guid id);
        Task<ReservationItemResponse> CreateReservationItemAsync(CreateClassReservationRequest request);

        Task<List<ReservationItemResponse>> CreateListReservationItemAsync(List<CreateClassReservationRequest> request);

        Task<bool> DeleteReservationItemAsync(Guid id);
        Task<ClassReservationResponse?> UpdateReservationItemAsync(UpdateClassReservationRequest request);
    }
}
