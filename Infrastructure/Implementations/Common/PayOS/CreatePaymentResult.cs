using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record CreatePaymentResult(
        String bin,
        String accountNumber,
        int amount,
        String description,
        long orderCode,
        String paymentLinkId,
        String status,
        String checkoutUrl,
        String qrCode
    );
}
