using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Invoice.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_InvoiceService : BaseService<FIN_Invoice, InvoiceResponse, UpdateInvoiceRequest, CreateInvoiceRequest>, IFIN_InvoiceService
	{
		private readonly IACAD_ReservationItemRepository _reservationItemRepository;
        public FIN_InvoiceService(IFIN_InvoiceRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IACAD_ReservationItemRepository reservationItemRepository)
			: base(repository, unitOfWork, mapper)
		{
			_reservationItemRepository = reservationItemRepository;
        }
		public async Task<FIN_Invoice?> CreateInvolcesTopay(Guid reservationId,Guid studentId)
		{
			var reservationItems = await _reservationItemRepository.GetByIdAsync(reservationId);
			if (reservationItems == null)
			{
				return null;
            }
			var invoice = new FIN_Invoice
			{
				Id = Guid.NewGuid(),
				StudentID = studentId,

            };
			_repository.Add(invoice);
			await _unitOfWork.SaveChangesAsync();
            return invoice;

        }
	}
}


