using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record PaymentData(
        long orderCode,
        int amount,
        String description,
        List<ItemData> items,
        String cancelUrl,
        String returnUrl,
        String? signature = null,
        String? buyerName = null,
        String? buyerEmail = null,
        String? buyerPhone = null,
        String? buyerAddress = null,
        int? expiredAt = null
    );
}
