using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.PayOS
{
    public record ItemData(
        String name,
        int quantity,
        int price
    );
}
