using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record PaymentLinkInformation(
    String id,
    long orderCode,
    int amount,
    int amountPaid,
    int amountRemaining,
    String status,
    String createdAt,
    List<Transaction> transactions,
    String? canceledAt,
    String? cancellationReason
    );
}
