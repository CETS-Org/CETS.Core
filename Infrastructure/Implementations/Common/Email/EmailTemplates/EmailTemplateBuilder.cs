using Application.Interfaces.Common.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.Email.EmailTemplates
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        private readonly string _templateRoot;

        public EmailTemplateBuilder()
        {
            _templateRoot = Path.Combine(AppContext.BaseDirectory, "EmailTemplates", "Templates");
        }

        private string LoadTemplate(string fileName)
        {
            string path = Path.Combine(_templateRoot, fileName);
            return File.ReadAllText(path);
        }

        // ======================
        // 1. Class Postpone Email
        // ======================
        public string BuildClassPostponeEmail(
            string studentName,
            string className,
            DateTime startDate,
            string continueUrl,
            string refundUrl)
        {
            var template = LoadTemplate("ClassPostpone.html");

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{ClassName}}", className)
                .Replace("{{StartDate}}", startDate.ToString("dd MMM yyyy"))
                .Replace("{{ContinueUrl}}", continueUrl)
                .Replace("{{RefundUrl}}", refundUrl);
        }

        // ======================
        // 2. Refund Confirmation Email
        // ======================
        public string BuildRefundConfirmationEmail(
            string studentName,
            string className,
            decimal refundAmount,
            string refundMethod,
            DateTime requestDate,
            DateTime processedDate)
        {
            var template = LoadTemplate("RefundConfirmation.html");

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{ClassName}}", className)
                .Replace("{{RefundAmount}}", refundAmount.ToString("N0") + " VND")
                .Replace("{{RefundMethod}}", refundMethod)
                .Replace("{{RequestDate}}", requestDate.ToString("dd MMM yyyy"))
                .Replace("{{ProcessedDate}}", processedDate.ToString("dd MMM yyyy"));
        }
        // ======================
        // 3. Confirm watiting for class email
        // ======================

        public string BuildContinueWaitingEmail(
            string studentName,
            string className,
            DateTime startDate)
        {
            string template = LoadTemplate("ContinueWaitingForClass.html");

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{ClassName}}", className)
                .Replace("{{StartDate}}", startDate.ToString("dd MMM yyyy"));
        }

        // ======================
        // 4. Dropout Request Submitted Email
        // ======================
        public string BuildDropoutRequestSubmittedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string submissionDate)
        {
            var template = LoadTemplate("DropoutRequestSubmitted.html");

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{RequestType}}", requestType)
                .Replace("{{EffectiveDate}}", effectiveDate)
                .Replace("{{SubmissionDate}}", submissionDate)
                .Replace("{{Year}}", DateTime.Now.Year.ToString());
        }

        // ======================
        // 5. Dropout Request Approved Email
        // ======================
        public string BuildDropoutRequestApprovedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string status,
            string processedDate,
            string? staffComment = null)
        {
            var template = LoadTemplate("DropoutRequestApproved.html");

            var staffCommentHtml = string.IsNullOrEmpty(staffComment)
                ? ""
                : $"<p style=\"font-size:14px;color:#333;margin:8px 0;\"><strong>Staff Comment:</strong> {staffComment}</p>";

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{RequestType}}", requestType)
                .Replace("{{EffectiveDate}}", effectiveDate)
                .Replace("{{Status}}", status)
                .Replace("{{ProcessedDate}}", processedDate)
                .Replace("{{StaffComment}}", staffCommentHtml)
                .Replace("{{Year}}", DateTime.Now.Year.ToString());
        }

        // ======================
        // 6. Dropout Request Completed Email
        // ======================
        public string BuildDropoutRequestCompletedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string status,
            string processedDate,
            string? staffComment = null)
        {
            var template = LoadTemplate("DropoutRequestCompleted.html");

            var staffCommentHtml = string.IsNullOrEmpty(staffComment)
                ? ""
                : $"<p style=\"font-size:14px;color:#333;margin:8px 0;\"><strong>Staff Comment:</strong> {staffComment}</p>";

            return template
                .Replace("{{StudentName}}", studentName)
                .Replace("{{RequestType}}", requestType)
                .Replace("{{EffectiveDate}}", effectiveDate)
                .Replace("{{Status}}", status)
                .Replace("{{ProcessedDate}}", processedDate)
                .Replace("{{StaffComment}}", staffCommentHtml)
                .Replace("{{Year}}", DateTime.Now.Year.ToString());
        }
        // ======================
        // 7. Attendance Warning Email
        // ======================
        public string BuildAttendanceWarningEmail(
         string studentName,
         string courseName,
         string className,
         int absentCount,
         int totalSessions,
         int maxAbsent)
            {
                var template = LoadTemplate("AttendanceWarning.html");

                return template
                    .Replace("{{StudentName}}", studentName)
                    .Replace("{{CourseName}}", courseName)
                    .Replace("{{ClassName}}", className)
                    .Replace("{{AbsentCount}}", absentCount.ToString())
                    .Replace("{{TotalSessions}}", totalSessions.ToString())
                    .Replace("{{MaxAbsent}}", maxAbsent.ToString());
            }



    }
}
