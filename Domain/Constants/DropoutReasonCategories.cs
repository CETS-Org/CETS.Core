using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class DropoutReasonCategories
    {
        public const string PersonalReason = "PersonalReason";
        public const string FinancialReason = "FinancialReason";
        public const string UnsatisfiedWithCourse = "UnsatisfiedWithCourse";
        public const string UnsatisfiedWithTeacher = "UnsatisfiedWithTeacher";
        public const string ScheduleConflict = "ScheduleConflict";
        public const string MovingAway = "MovingAway";
        public const string HealthIssue = "HealthIssue";
        public const string NoLongerInterested = "NoLongerInterested";
        public const string FoundAnotherCentre = "FoundAnotherCentre";
        public const string Other = "Other";

        public static readonly string[] All = new[]
        {
            PersonalReason,
            FinancialReason,
            UnsatisfiedWithCourse,
            UnsatisfiedWithTeacher,
            ScheduleConflict,
            MovingAway,
            HealthIssue,
            NoLongerInterested,
            FoundAnotherCentre,
            Other
        };
    }
}

