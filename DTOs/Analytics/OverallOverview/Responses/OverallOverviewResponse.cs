namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Overall overview analytics response containing all key metrics
    /// Organized by business insight categories
    /// </summary>
    public class OverallOverviewResponse
    {
        /// <summary>
        /// CENTER PERFORMANCE - Operational Efficiency
        /// Tổng hợp công suất trung tâm, tỉ lệ sử dụng phòng học, lớp mở/đóng
        /// </summary>
        public CenterPerformanceResponse CenterPerformance { get; set; } = new();

        /// <summary>
        /// GROWTH & RETENTION - Student Lifecycle
        /// Theo dõi tăng trưởng số lượng học viên, tỉ lệ rời bỏ, reactivation
        /// </summary>
        public GrowthRetentionResponse GrowthRetention { get; set; } = new();

        /// <summary>
        /// REVENUE & FINANCE - Financial Health
        /// Tổng quan tài chính (doanh thu theo tháng, thu học phí, hoàn tiền, dự báo)
        /// </summary>
        public RevenueFinanceResponse RevenueFinance { get; set; } = new();

        /// <summary>
        /// ENGAGEMENT & SATISFACTION - Customer Experience
        /// Mức độ hài lòng của học viên qua feedback & đánh giá
        /// </summary>
        public EngagementSatisfactionResponse EngagementSatisfaction { get; set; } = new();

        /// <summary>
        /// SYSTEM HEALTH - Platform Performance
        /// Đánh giá hiệu suất hoạt động toàn hệ thống
        /// </summary>
        public SystemHealthResponse SystemHealth { get; set; } = new();

        /// <summary>
        /// Timestamp when the data was generated
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// [DEPRECATED] Old structure - kept for backward compatibility
        /// Recommend using new category-based properties above
        /// </summary>
        [Obsolete("Use CenterPerformance, GrowthRetention, RevenueFinance, EngagementSatisfaction, SystemHealth instead")]
        public LegacyMetricsResponse? Legacy { get; set; }
    }

    /// <summary>
    /// Legacy metrics structure for backward compatibility
    /// </summary>
    public class LegacyMetricsResponse
    {
        public StudentMetricsResponse? StudentMetrics { get; set; }
        public FinancialMetricsResponse? FinancialMetrics { get; set; }
        public CourseClassMetricsResponse? CourseClassMetrics { get; set; }
        public TeacherMetricsResponse? TeacherMetrics { get; set; }
        public EventMetricsResponse? EventMetrics { get; set; }
        public FeedbackMetricsResponse? FeedbackMetrics { get; set; }
    }
}


