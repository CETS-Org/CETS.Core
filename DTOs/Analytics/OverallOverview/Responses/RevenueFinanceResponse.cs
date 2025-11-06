namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Revenue & Finance Metrics
    /// Tổng quan tài chính (doanh thu theo tháng, thu học phí, hoàn tiền, dự báo doanh thu)
    /// </summary>
    public class RevenueFinanceResponse
    {
        // Revenue Trends
        /// <summary>
        /// Total revenue (all time)
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Revenue this month
        /// </summary>
        public decimal RevenueThisMonth { get; set; }

        /// <summary>
        /// Revenue last month
        /// </summary>
        public decimal RevenueLastMonth { get; set; }

        /// <summary>
        /// Revenue this year (YTD)
        /// </summary>
        public decimal RevenueThisYear { get; set; }

        /// <summary>
        /// Month-over-month revenue growth (%)
        /// </summary>
        public decimal MonthOverMonthGrowth { get; set; }

        /// <summary>
        /// Revenue trend: "Growing", "Stable", "Declining"
        /// </summary>
        public string RevenueTrend { get; set; } = string.Empty;

        // Tuition Collection
        /// <summary>
        /// Total tuition fees collected (paid invoices)
        /// </summary>
        public decimal TuitionCollected { get; set; }

        /// <summary>
        /// Tuition collected this month
        /// </summary>
        public decimal TuitionCollectedThisMonth { get; set; }

        /// <summary>
        /// Average tuition per student
        /// </summary>
        public decimal AverageTuitionPerStudent { get; set; }

        /// <summary>
        /// Collection efficiency rate (%)
        /// (Collected / Total billed) * 100
        /// </summary>
        public decimal CollectionEfficiencyRate { get; set; }

        // Pending Payments
        /// <summary>
        /// Total amount of pending payments
        /// </summary>
        public decimal PendingPaymentAmount { get; set; }

        /// <summary>
        /// Number of pending invoices
        /// </summary>
        public int PendingInvoicesCount { get; set; }

        /// <summary>
        /// Overdue payment amount
        /// </summary>
        public decimal OverduePaymentAmount { get; set; }

        /// <summary>
        /// Number of overdue invoices
        /// </summary>
        public int OverdueInvoicesCount { get; set; }

        /// <summary>
        /// Overdue rate (%)
        /// </summary>
        public decimal OverdueRate { get; set; }

        // Refunds
        /// <summary>
        /// Total refund volume (amount)
        /// </summary>
        public decimal TotalRefundVolume { get; set; }

        /// <summary>
        /// Refunds issued this month
        /// </summary>
        public decimal RefundsThisMonth { get; set; }

        /// <summary>
        /// Number of refund transactions
        /// </summary>
        public int RefundTransactionsCount { get; set; }

        /// <summary>
        /// Refund rate (%)
        /// (Refunds / Total revenue) * 100
        /// </summary>
        public decimal RefundRate { get; set; }

        // Revenue Forecast
        /// <summary>
        /// Forecasted revenue for next month
        /// Based on historical trends and current pipeline
        /// </summary>
        public decimal ForecastedRevenueNextMonth { get; set; }

        /// <summary>
        /// Forecasted revenue for next quarter
        /// </summary>
        public decimal ForecastedRevenueNextQuarter { get; set; }

        /// <summary>
        /// Expected revenue from pending enrollments
        /// </summary>
        public decimal PipelineRevenue { get; set; }

        /// <summary>
        /// Confidence level of forecast: "High", "Medium", "Low"
        /// </summary>
        public string ForecastConfidence { get; set; } = string.Empty;

        // Financial Health
        /// <summary>
        /// Average revenue per student (ARPU)
        /// </summary>
        public decimal AverageRevenuePerStudent { get; set; }

        /// <summary>
        /// Revenue growth rate year-over-year (%)
        /// </summary>
        public decimal YearOverYearGrowthRate { get; set; }

        /// <summary>
        /// Payment method distribution
        /// </summary>
        public Dictionary<string, int> PaymentMethodDistribution { get; set; } = new();
    }
}




