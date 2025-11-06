namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Financial metrics for overall overview dashboard
    /// </summary>
    public class FinancialMetricsResponse
    {
        /// <summary>
        /// Total revenue from all paid invoices
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Revenue for current month
        /// </summary>
        public decimal MonthlyRevenue { get; set; }

        /// <summary>
        /// Revenue for current year
        /// </summary>
        public decimal YearlyRevenue { get; set; }

        /// <summary>
        /// Average revenue per student
        /// </summary>
        public decimal AverageRevenuePerStudent { get; set; }

        /// <summary>
        /// Number of paid invoices
        /// </summary>
        public int PaidInvoicesCount { get; set; }

        /// <summary>
        /// Number of pending invoices
        /// </summary>
        public int PendingInvoicesCount { get; set; }

        /// <summary>
        /// Number of overdue invoices
        /// </summary>
        public int OverdueInvoicesCount { get; set; }

        /// <summary>
        /// Percentage of invoices that are overdue
        /// </summary>
        public decimal OverdueRate { get; set; }

        /// <summary>
        /// Number of invoices with installment payment
        /// </summary>
        public int InstallmentInvoicesCount { get; set; }

        /// <summary>
        /// Total amount from installment invoices
        /// </summary>
        public decimal InstallmentRevenue { get; set; }

        /// <summary>
        /// Revenue growth rate compared to last month (percentage)
        /// </summary>
        public decimal MonthOverMonthRevenueGrowth { get; set; }
    }
}





