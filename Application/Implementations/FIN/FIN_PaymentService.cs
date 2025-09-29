using Application.Interfaces.CORE;
using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
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
        public FIN_PaymentService(IFIN_PaymentRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICORE_LookUpService lookUpService, IFIN_PaymentWebhookService paymentWebhookService, IFIN_InvoiceService invoiceService)
			: base(repository, unitOfWork, mapper)
		{
			_lookUpService = lookUpService;
			_paymentWebhookService = paymentWebhookService;
			_invoiceService = invoiceService;
        }

		public async Task<FIN_Payment?> CreateMonthlyPayment(Guid invoiceId)
		{
            var invoice = await _invoiceService.GetByIdAsync(invoiceId);
            if (invoice != null)
			{
                var gateway = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.Gateway);
                var payos = gateway?.Where(x => x.Code == "PayOs").FirstOrDefault();
                var paymentmethod = await _lookUpService.GetByTypeCodeAsync(LookUpTypes.PaymentMethod);
                var onlineBanking = paymentmethod?.Where(x => x.Code == "Bank").FirstOrDefault();
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
                var firstStatus = paymentStatus?.Where(x => x.Code == "1stPaid").FirstOrDefault();
                var invoiceStatus = await _lookUpService.GetByIdAsync(invoice.InvoiceStatusID);
                if (invoiceStatus.Code == "Pending" && invoice.IsInstallment)
                {
                    await _invoiceService.updateInvoiceStatus(invoice.Id, firstStatus.LookUpId);
                }
                await _unitOfWork.SaveChangesAsync();
                return payment;
            } 
            return null;
        }

    }
}


