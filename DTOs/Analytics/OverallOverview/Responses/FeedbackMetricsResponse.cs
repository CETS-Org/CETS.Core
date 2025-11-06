namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Feedback and satisfaction metrics for overall overview dashboard
    /// </summary>
    public class FeedbackMetricsResponse
    {
        /// <summary>
        /// Total number of feedbacks
        /// </summary>
        public int TotalFeedbacks { get; set; }

        /// <summary>
        /// Overall average rating across all feedbacks
        /// </summary>
        public decimal OverallAverageRating { get; set; }

        /// <summary>
        /// Average rating for courses
        /// </summary>
        public decimal CourseAverageRating { get; set; }

        /// <summary>
        /// Number of course feedbacks
        /// </summary>
        public int CourseFeedbackCount { get; set; }

        /// <summary>
        /// Average rating for teachers
        /// </summary>
        public decimal TeacherAverageRating { get; set; }

        /// <summary>
        /// Number of teacher feedbacks
        /// </summary>
        public int TeacherFeedbackCount { get; set; }

        /// <summary>
        /// Number of 5-star ratings
        /// </summary>
        public int FiveStarCount { get; set; }

        /// <summary>
        /// Number of 4-star ratings
        /// </summary>
        public int FourStarCount { get; set; }

        /// <summary>
        /// Number of 3-star ratings
        /// </summary>
        public int ThreeStarCount { get; set; }

        /// <summary>
        /// Number of 2-star ratings
        /// </summary>
        public int TwoStarCount { get; set; }

        /// <summary>
        /// Number of 1-star ratings
        /// </summary>
        public int OneStarCount { get; set; }

        /// <summary>
        /// Net Promoter Score (Promoters % - Detractors %)
        /// Promoters: 4-5 stars, Detractors: 1-2 stars
        /// </summary>
        public decimal NetPromoterScore { get; set; }

        /// <summary>
        /// Customer Satisfaction Score (percentage of 4-5 star ratings)
        /// </summary>
        public decimal CustomerSatisfactionScore { get; set; }
    }
}





