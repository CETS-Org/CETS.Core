using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Settings
{
    public class SuspensionPolicySettings
    {
        public int MinDays { get; set; } = 7;
        public int MaxDays { get; set; } = 90;
        public int NoticePeriodDays { get; set; } = 7;
        public int MaxSuspensionsPerYear { get; set; } = 2;
        public int RequireDocumentOverDays { get; set; } = 30;
        public int AwaitingReturnGraceDays { get; set; } = 14;
    }
}

