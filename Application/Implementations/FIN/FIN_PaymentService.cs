using Application.Interfaces.ACAD;
using Application.Interfaces.CORE;
using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Payment.Requests;
using DTOs.FIN.FIN_Payment.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentService : BaseService<FIN_Payment, PaymentResponse, UpdatePaymentRequest, CreatePaymentRequest>, IFIN_PaymentService
	{
        private readonly ICORE_LookUpService _lookUpService;
		private readonly IFIN_PaymentWebhookService _paymentWebhookService;
		private readonly IFIN_InvoiceService _invoiceService;
        private readonly IACAD_ReservationItemService _reservationItemService;
        private readonly IACAD_ClassReservationService _ClassReservationService;
        private readonly IACAD_EnrollmentRepository _enrollmentRepository;
        public FIN_PaymentService(IFIN_PaymentRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICORE_LookUpService lookUpService, IFIN_PaymentWebhookService paymentWebhookService, IFIN_InvoiceService invoiceService, IACAD_ReservationItemService reservationItemService, IACAD_EnrollmentRepository enrollmentRepository, IACAD_ClassReservationService classReservationService)
			: base(repository, unitOfWork, mapper)
		{
			_lookUpService = lookUpService;
			_paymentWebhookService = paymentWebhookService;
			_invoiceService = invoiceService;
            _reservationItemService = reservationItemService;
            _enrollmentRepository = enrollmentRepository;
            _ClassReservationService = classReservationService;
        }

		public async Task<FIN_Payment?> CreateMonthlyPayment(Guid invoiceId,Guid studentId, Guid reservationItemId)
		{
            var invoice = await _invoiceService.GetByIdAsync(invoiceId);
            if (invoice != null)
			{
                var gateway = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.Gateway);
                var payos = gateway?.Where(x => x.Code == "PayOs").FirstOrDefault();
                var paymentmethod = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.PaymentMethod);
                var onlineBanking = paymentmethod?.Where(x => x.Code == "Bank").FirstOrDefault();
                //Create payment record
                var payment = new FIN_Payment
                {
                    Id = Guid.NewGuid(),
                    InvoiceID = invoiceId,
                    Amount = invoice.TotalAmount,
                    PaymentDate = DateTime.Now,
                    PaymentMethodID = onlineBanking.LookUpId,
                    GatewayID = payos.LookUpId,
                    CreatedAt = DateTime.Now
                };
                _repository.Add(payment);
                var paymentStatus = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.PaymentStatus);
                
                var invoiceStatus = await _lookUpService.GetByIdAsync(invoice.InvoiceStatusID);
                var reservationItem = await _reservationItemService.GetReservationItemByIdAsync(reservationItemId);
                //add invoiceId to reservation item
                //await _reservationItemService.UpdateReservationItemInvoiceIdAsync(reservationItemId, invoiceId);
                //update invoice status if pending and installment
                if (invoiceStatus.Code == "Pending")
                {
                    //update instalment pay status
                    var firstStatus = paymentStatus?.Where(x => x.Code == "1stPaid").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, firstStatus.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Completed").FirstOrDefault();
                    await _ClassReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                    
                    //Create enrollment record only when first payment is made
                    var enrollmentStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.EnrollmentStatus);
                    var enrollmentStatus = enrollmentStatusLookup?.Where(x => x.Code == "Pending").FirstOrDefault();
                    var enrollment = new ACAD_Enrollment
                    {
                        Id = Guid.NewGuid(),
                        StudentID = studentId,
                        EnrollmentStatusID = enrollmentStatus.LookUpId,
                        CreatedAt = DateTime.Now,
                        CourseID = reservationItem.CourseId,
                        InvoiceID = invoiceId
                    };
                    _enrollmentRepository.Add(enrollment);
                }
                if (invoiceStatus.Code == "1stPaid" )
                {
                    //update 2nd second instalment pay status
                    var firstStatus = paymentStatus?.Where(x => x.Code == "2ndPaid").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, firstStatus.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Completed").FirstOrDefault();
                    await _ClassReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                }

                //update invoice status if pending and full payment
                if (invoiceStatus.Code == "Pending" && reservationItem.PlanType == "OneTime")
                {
                    //update payment complete status
                    var paymentComplete = paymentStatus?.Where(x => x.Code == "PaymentComplete").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, paymentComplete.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Complete").FirstOrDefault();
                    await _ClassReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                    
                    //Create enrollment record only when first payment is made
                    var enrollmentStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.EnrollmentStatus);
                    var enrollmentStatus = enrollmentStatusLookup?.Where(x => x.Code == "Pending").FirstOrDefault();
                    var enrollment = new ACAD_Enrollment
                    {
                        Id = Guid.NewGuid(),
                        StudentID = studentId,
                        EnrollmentStatusID = enrollmentStatus.LookUpId,
                        CreatedAt = DateTime.Now,
                        CourseID = reservationItem.CourseId,
                        InvoiceID = invoiceId
                    };
                    _enrollmentRepository.Add(enrollment);
                }
                await _unitOfWork.SaveChangesAsync();
                return payment;
            } 
            return null;
        }

    }
}


