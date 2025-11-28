using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class SuspensionStatuses
    {
        public const string Draft = "Draft";
        public const string Pending = "Pending";
        public const string NeedInfo = "NeedInfo";
        public const string UnderReview = "UnderReview";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Suspended = "Suspended";
        public const string AwaitingReturn = "AwaitingReturn";
        public const string Completed = "Completed";
        public const string AutoDroppedOut = "AutoDroppedOut";
        public const string Expired = "Expired";
    }
}

