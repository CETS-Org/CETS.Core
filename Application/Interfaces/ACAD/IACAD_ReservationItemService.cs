using Domain.Entities;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_ReservationItem.Requests;
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
        IQueryable<ReservationItemResponse?> GetReservationItemByReservationId(Guid id);
        Task<ReservationItemResponse> CreateReservationItemAsync(CreateReservationItemRequests request);

        Task<List<ReservationItemResponse>> CreateListReservationItemAsync(List<CreateReservationItemRequests> request);

        Task<bool> DeleteReservationItemAsync(Guid id);
        Task<ReservationItemResponse?> UpdateReservationItemAsync(UpdateReservationItemRequest request);
        Task<ReservationItemResponse?> UpdateReservationItemInvoiceIdAsync(Guid id, Guid invoiceId);
    }
}
