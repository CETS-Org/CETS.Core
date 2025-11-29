using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }

    public enum RequestType
    {
        [Priority(1)]
        SystemDown,

        [Priority(1)]
        PaymentError,

        [Priority(2)]
        RoomBookingError,

        [Priority(2)]
        TeachingScheduleError,

        [Priority(3)]
        FileUploadError,

        [Priority(4)]
        ProfileIssue,
    }


}
