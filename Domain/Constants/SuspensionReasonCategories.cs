using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class SuspensionReasonCategories
    {
        public const string Health = "Health";
        public const string FamilyIssue = "FamilyIssue";
        public const string ScheduleConflict = "ScheduleConflict";
        public const string TemporaryRelocation = "TemporaryRelocation";
        public const string FinancialDifficulty = "FinancialDifficulty";
        public const string Other = "Other";

        public static readonly string[] All = new[]
        {
            Health,
            FamilyIssue,
            ScheduleConflict,
            TemporaryRelocation,
            FinancialDifficulty,
            Other
        };
    }
}

