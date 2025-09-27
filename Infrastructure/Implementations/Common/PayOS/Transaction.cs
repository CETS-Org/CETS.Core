using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record Transaction(
        String reference,
        int amount,
        String accountNumber,
        String description,
        String transactionDateTime,
        String? virtualAccountName,
        String? virtualAccountNumber,
        String? counterAccountBankId,
        String? counterAccountBankName,
        String? counterAccountName,
        String? counterAccountNumber
    );
}
