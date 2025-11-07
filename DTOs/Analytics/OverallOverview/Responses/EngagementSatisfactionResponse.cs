namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Engagement & Satisfaction Metrics
    /// Mức độ hài lòng của học viên qua feedback & đánh giá
    /// </summary>
    public class EngagementSatisfactionResponse
    {
        // Overall Satisfaction
        /// <summary>
        /// Overall feedback score (average rating)
        /// </summary>
        public decimal OverallFeedbackScore { get; set; }

        /// <summary>
        /// Average rating across all feedbacks (1-5 scale)
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        /// Total number of feedbacks received
        /// </summary>
        public int TotalFeedbacksReceived { get; set; }

        /// <summary>
        /// Feedback response rate (%)
        /// (Feedbacks / Total students) * 100
        /// </summary>
        public decimal FeedbackResponseRate { get; set; }

        // Student Satisfaction
        /// <summary>
        /// Student satisfaction score (%)
        /// Based on 4-5 star ratings
        /// </summary>
        public decimal StudentSatisfactionScore { get; set; }

        /// <summary>
        /// Course satisfaction average rating
        /// </summary>
        public decimal CourseSatisfactionRating { get; set; }

        /// <summary>
        /// Teacher satisfaction average rating
        /// </summary>
        public decimal TeacherSatisfactionRating { get; set; }

        /// <summary>
        /// Facility/environment satisfaction rating
        /// </summary>
        public decimal FacilitySatisfactionRating { get; set; }

        // Net Promoter Score (NPS)
        /// <summary>
        /// Net Promoter Score (-100 to +100)
        /// (% Promoters - % Detractors)
        /// </summary>
        public decimal NetPromoterScore { get; set; }

        /// <summary>
        /// Number of promoters (rating 4-5)
        /// </summary>
        public int PromotersCount { get; set; }

        /// <summary>
        /// Number of passives (rating 3)
        /// </summary>
        public int PassivesCount { get; set; }

        /// <summary>
        /// Number of detractors (rating 1-2)
        /// </summary>
        public int DetractorsCount { get; set; }

        /// <summary>
        /// NPS category: "Excellent" (>70), "Good" (50-70), "Fair" (30-50), "Poor" (<30)
        /// </summary>
        public string NPSCategory { get; set; } = string.Empty;

        // Rating Distribution
        /// <summary>
        /// Percentage of 5-star ratings
        /// </summary>
        public decimal FiveStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 4-star ratings
        /// </summary>
        public decimal FourStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 3-star ratings
        /// </summary>
        public decimal ThreeStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 2-star ratings
        /// </summary>
        public decimal TwoStarPercentage { get; set; }

        /// <summary>
        /// Percentage of 1-star ratings
        /// </summary>
        public decimal OneStarPercentage { get; set; }

        // Engagement Metrics
        /// <summary>
        /// Average attendance rate across all classes (%)
        /// </summary>
        public decimal AverageAttendanceRate { get; set; }

        /// <summary>
        /// Assignment submission rate (%)
        /// </summary>
        public decimal AssignmentSubmissionRate { get; set; }

        /// <summary>
        /// Student participation score (composite)
        /// Based on attendance, submissions, feedback
        /// </summary>
        public decimal StudentParticipationScore { get; set; }

        // Satisfaction Trends
        /// <summary>
        /// Satisfaction trend: "Improving", "Stable", "Declining"
        /// </summary>
        public string SatisfactionTrend { get; set; } = string.Empty;

        /// <summary>
        /// Satisfaction change from last period (%)
        /// </summary>
        public decimal SatisfactionChangeRate { get; set; }

        // Complaint/Issue Metrics
        /// <summary>
        /// Number of complaints/negative feedbacks
        /// </summary>
        public int ComplaintsCount { get; set; }

        /// <summary>
        /// Complaint resolution rate (%)
        /// </summary>
        public decimal ComplaintResolutionRate { get; set; }
    }
}






