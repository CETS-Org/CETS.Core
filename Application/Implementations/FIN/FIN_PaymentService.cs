using Application.Interfaces.ACAD;
using Application.Interfaces.CORE;
using Application.Interfaces.ExternalServices.Email;
using Application.Interfaces.FIN;
using Application.Interfaces.IDN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.FIN;
using Domain.Interfaces.IDN;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Payment.Requests;
using DTOs.FIN.FIN_Payment.Responses;
using DTOs.FIN.FIN_PaymentWebhook.Requests;
using System.Linq;
using System.Text.Json;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentService : BaseService<FIN_Payment, PaymentResponse, UpdatePaymentRequest, CreatePaymentRequest>, IFIN_PaymentService
	{
        private readonly ICORE_LookUpService _lookUpService;
		private readonly IFIN_PaymentWebhookService _paymentWebhookService;
		private readonly IFIN_InvoiceService _invoiceService;
        private readonly IACAD_ReservationItemService _reservationItemService;
        private readonly IACAD_ClassReservationService _classReservationService;
        private readonly IACAD_EnrollmentRepository _enrollmentRepository;
        private readonly IMailService _mailService;
        private readonly IIDN_AccountService _accountService;
        private readonly IFIN_InvoiceItemService _invoiceItemService;
        public FIN_PaymentService(IFIN_PaymentRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICORE_LookUpService lookUpService, 
            IFIN_PaymentWebhookService paymentWebhookService, IFIN_InvoiceService invoiceService, IACAD_ReservationItemService reservationItemService, 
            IACAD_EnrollmentRepository enrollmentRepository, IACAD_ClassReservationService classReservationService, IMailService mailService, IIDN_AccountService accountService,
            IFIN_InvoiceItemService invoiceItemService)
			: base(repository, unitOfWork, mapper)
		{
			_lookUpService = lookUpService;
			_paymentWebhookService = paymentWebhookService;
			_invoiceService = invoiceService;
            _reservationItemService = reservationItemService;
            _enrollmentRepository = enrollmentRepository;
            _classReservationService = classReservationService;
            _mailService = mailService;
            _accountService = accountService;
            _invoiceItemService = invoiceItemService;
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

                //Create payment webhook record
                var webhookPayload = new
                {
                    paymentId = payment.Id,
                    invoiceId = invoiceId,
                    studentId = studentId,
                    reservationItemId = reservationItemId,
                    amount = invoice.TotalAmount,
                    paymentDate = DateTime.Now,
                    status = "PAID"
                };

                var createWebhookRequest = new CreatePaymentWebhookRequest
                {
                    PaymentID = payment.Id,
                    EventId = Guid.NewGuid(),
                    GatewayID = payos.LookUpId,
                    EventType = "payment.success",
                    ReceivedAt = DateTime.Now,
                    Payload = JsonSerializer.Serialize(webhookPayload)
                };
                await _paymentWebhookService.CreateAsync(createWebhookRequest);

                //Send billing mail to user
                var account = await _accountService.GetAccountByIdAsync(studentId);
                var invoiceItemList = await _invoiceItemService.GetByInvoiceIdAsync(invoice.Id);
                var invoiceItem = invoiceItemList.FirstOrDefault();
                if (account != null)
                {
                    string subject = $"Course Payment Confirmation - {invoice.InvoiceNumber}";
                    string body = $@"
                        <div style='font-family:Arial, sans-serif; font-size:16px; color:#333; padding:20px; max-width:600px; margin:0 auto;'>
                            <div style='text-align:center; margin-bottom:30px;'>
                                <h1 style='color:#007bff; margin:0;'>Course Payment Confirmation</h1>
                                <p style='color:#666; margin:5px 0;'>Thank you for your payment!</p>
                            </div>
                    
                            <div style='background:#f8f9fa; padding:20px; border-radius:8px; margin-bottom:20px;'>
                                <h3 style='color:#007bff; margin-top:0;'>Payment Details</h3>
                                <p><strong>Invoice Number:</strong> {invoice.InvoiceNumber}</p>
                                <p><strong>Payment Date:</strong> {DateTime.Now:MM/dd/yyyy}</p>
                                <p><strong>Status:</strong> <span style='color:#28a745; font-weight:bold;'>Paid Successfully</span></p>
                            </div>

                            <div style='background:#f8f9fa; padding:20px; border-radius:8px; margin-bottom:20px;'>
                                <h3 style='color:#007bff; margin-top:0;'>Course Information</h3>
                                <div style='border-bottom:1px solid #eee; padding:10px 0;'>
                                    <p style='margin:5px 0; font-weight:bold;'>{invoiceItem?.Course?.CourseName ?? "Course"}</p>
                                    <p style='margin:5px 0; color:#666;'>Quantity: {invoiceItem?.Quantity ?? 1}</p>
                                    <p style='margin:5px 0; color:#666;'>Price: ${invoiceItem?.UnitPrice}</p>
                                </div>
                            </div>

                            <div style='background:#e8f5e8; padding:20px; border-radius:8px; margin-bottom:20px; text-align:center;'>
                                <h2 style='color:#28a745; margin:0 0 10px 0;'>Total Paid: ${invoice.TotalAmount}</h2>
                                <p style='color:#155724; margin:0;'>Your course enrollment is now confirmed!</p>
                            </div>

                            <div style='text-align:center; margin-top:30px; padding-top:20px; border-top:1px solid #eee;'>
                                <p style='color:#666; margin:5px 0;'>If you have any questions, please contact us.</p>
                                <p style='color:#666; margin:5px 0;'>Email: support@cets.com | Phone: 1900-xxxx</p>
                                <p style='color:#666; margin:5px 0;'>Best regards,<br/><strong>CETS Team</strong></p>
                            </div>
                        </div>";

                    if (account != null && account.Email != null)
                        await _mailService.SendEmailAsync(account.Email, subject, body);
                }
                

                var invoiceStatusForUpdate = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.InvoiceStatus);               
                var invoiceStatus = await _lookUpService.GetByIdAsync(invoice.InvoiceStatusID);
                var reservationItem = await _reservationItemService.GetReservationItemByIdAsync(reservationItemId);

                //update invoice status if pending and installment
                if (invoiceStatus.Code == "Pending")
                {
                    //update instalment pay status
                    var firstStatus = invoiceStatusForUpdate?.Where(x => x.Code == "1stPaid").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, firstStatus.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Completed").FirstOrDefault();
                    await _classReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                    
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
                    var firstStatus = invoiceStatusForUpdate?.Where(x => x.Code == "2ndPaid").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, firstStatus.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Completed").FirstOrDefault();
                    await _classReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                }

                //update invoice status if pending and full payment
                if (invoiceStatus.Code == "Pending" && reservationItem.PlanType == "OneTime")
                {
                    //update payment complete status
                    var paymentComplete = invoiceStatusForUpdate?.Where(x => x.Code == "Paid").FirstOrDefault();
                    await _invoiceService.updateInvoiceStatus(invoice.Id, paymentComplete.LookUpId);
                    var ReservationStatusLookup = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.ReservationStatus);
                    var reservationStatus = ReservationStatusLookup?.Where(x => x.Code == "Complete").FirstOrDefault();
                    await _classReservationService.UpdateReservationStatusAsync(reservationItem.ClassReservationId, reservationStatus.LookUpId);
                    
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


