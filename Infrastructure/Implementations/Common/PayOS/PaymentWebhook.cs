using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record WebhookData(
    long orderCode,
    int amount,
    String description,
    String accountNumber,
    String reference,
    String transactionDateTime,
    String paymentLinkId,
    String code,
    String desc,
    String? counterAccountBankId,
    String? counterAccountBankName,
    String? counterAccountName,
    String? counterAccountNumber,
    String? virtualAccountName,
    String virtualAccountNumber
);

    public record WebhookType(
        String code,
        String desc,
        Boolean success,
        //WebhookDataType webhookDataType,
        String signature
    );
}
