using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.Email.EmailTemplates
{
    public class EmailTemplateBuilder
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

    }
}
