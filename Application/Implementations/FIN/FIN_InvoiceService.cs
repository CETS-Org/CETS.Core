using Application.Interfaces.CORE;
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
		private readonly ICORE_LookUpService _lookUpService;
		private readonly IFIN_InvoiceRepository _InvoiceRepository;
		private readonly IFIN_InvoiceItemRepository _invoiceItemRepository;
        public FIN_InvoiceService(IFIN_InvoiceRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IACAD_ReservationItemRepository reservationItemRepository, ICORE_LookUpService lookUpService, IFIN_InvoiceRepository invoiceRepository, IFIN_InvoiceItemRepository invoiceItemRepository)
			: base(repository, unitOfWork, mapper)
		{
			_reservationItemRepository = reservationItemRepository;
			_lookUpService = lookUpService;
			_InvoiceRepository = invoiceRepository;
			_invoiceItemRepository = invoiceItemRepository;
        }
		public async Task<FIN_Invoice?> CreateInvolcesToMonthlyPay(Guid reservationItemId,Guid studentId)
		{
			var reservationItems = await _reservationItemRepository.GetByReservationIdAsync(reservationItemId);
			if (reservationItems == null)
			{
				return null;
            }
			var invoiceStatus = await _lookUpService.GetByIdAsync(Guid.Parse("E64A6CD6-0144-4868-8AA3-8895E2EC92E1"));
			var nextSequence = await _InvoiceRepository.GetNextSequenceInvoiceIdAsync();

            // Format InvoiceNumber: YYYY + padded sequence (6 digit)
            string invoiceNumber = $"{DateTime.Now.Year}{nextSequence.ToString("D6")}";

                var amount = reservationItems.Course.StandardPrice / 2;
                var invoice = new FIN_Invoice
                {
                    Id = Guid.NewGuid(),
                    StudentID = studentId,
                    InvoiceStatusID = invoiceStatus.LookUpId,
                    CreateDate = DateOnly.FromDateTime(DateTime.Now),
					Subtotal = amount,
					TaxAmount = 0,
                    TotalAmount = amount,
					CreatedAt = DateTime.Now,
					InvoiceSequence = nextSequence,
					InvoiceNumber = invoiceNumber,
					IsInstallment = true,
                };
                _repository.Add(invoice);
                await _unitOfWork.SaveChangesAsync();
				var invoiceItemFirst = new FIN_InvoiceItem
				{
					Id = Guid.NewGuid(),
					InvoiceID = invoice.Id,
					CourseID = reservationItems.CourseID,
					Quantity = 1,
					UnitPrice = amount,
					Subtotal = amount,
					Total = amount,
					DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                };
				_invoiceItemRepository.Add(invoiceItemFirst);
                var invoiceItemSecond = new FIN_InvoiceItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceID = invoice.Id,
                    CourseID = reservationItems.CourseID,
                    Quantity = 1,
                    UnitPrice = amount,
                    Subtotal = amount,
                    Total = amount,
                };
				_invoiceItemRepository.Add(invoiceItemSecond);
                await _unitOfWork.SaveChangesAsync();
                return invoice;
        }
		public async Task<FIN_Invoice> updateInvoiceStatus(Guid invoiceId, Guid statusId)
		{
			var invoice = await _repository.GetByIdAsync(invoiceId);
			if (invoice == null)
			{
				throw new Exception("Invoice not found");
			}
			invoice.InvoiceStatusID = statusId;
			_repository.Update(invoice);
			await _unitOfWork.SaveChangesAsync();
			return invoice;
        }
    }
}


