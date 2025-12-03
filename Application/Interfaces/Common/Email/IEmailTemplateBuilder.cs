using System;

namespace Application.Interfaces.Common.Email
{
    public interface IEmailTemplateBuilder
    {
        // Class Postpone Email
        string BuildClassPostponeEmail(
            string studentName,
            string className,
            DateTime startDate,
            string continueUrl,
            string refundUrl);

        // Refund Confirmation Email
        string BuildRefundConfirmationEmail(
            string studentName,
            string className,
            decimal refundAmount,
            string refundMethod,
            DateTime requestDate,
            DateTime processedDate);

        // Continue Waiting for Class Email
        string BuildContinueWaitingEmail(
            string studentName,
            string className,
            DateTime startDate);

        // Dropout Request Submitted Email
        string BuildDropoutRequestSubmittedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string submissionDate);

        // Dropout Request Approved Email
        string BuildDropoutRequestApprovedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string status,
            string processedDate,
            string? staffComment = null);

        // Dropout Request Completed Email
        string BuildDropoutRequestCompletedEmail(
            string studentName,
            string requestType,
            string effectiveDate,
            string status,
            string processedDate,
            string? staffComment = null);

        // Suspension Activated Email
        string BuildSuspensionActivatedEmail(
            string studentName,
            string startDate,
            string endDate,
            string expectedReturnDate,
            string reasonCategory);

        // Suspension Ended Email
        string BuildSuspensionEndedEmail(
            string studentName,
            string endDate,
            string expectedReturnDate,
            int gracePeriodDays);

        // Suspension Return Reminder Email
        string BuildSuspensionReturnReminderEmail(
            string studentName,
            string endDate,
            string expectedReturnDate,
            int daysUntilReturn);

        // Auto Dropout Email
        string BuildAutoDropoutEmail(
            string studentName,
            string endDate,
            string expectedReturnDate,
            int daysOverdue,
            int gracePeriodDays);
    }
}

