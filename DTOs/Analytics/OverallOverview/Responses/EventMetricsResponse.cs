namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Event metrics for overall overview dashboard
    /// </summary>
    public class EventMetricsResponse
    {
        /// <summary>
        /// Total number of events
        /// </summary>
        public int TotalEvents { get; set; }

        /// <summary>
        /// Number of upcoming events
        /// </summary>
        public int UpcomingEvents { get; set; }

        /// <summary>
        /// Number of ongoing events
        /// </summary>
        public int OngoingEvents { get; set; }

        /// <summary>
        /// Number of completed events
        /// </summary>
        public int CompletedEvents { get; set; }

        /// <summary>
        /// Total number of event registrations
        /// </summary>
        public int TotalRegistrations { get; set; }

        /// <summary>
        /// Number of registrations with check-in
        /// </summary>
        public int CheckedInRegistrations { get; set; }

        /// <summary>
        /// Event attendance rate (percentage)
        /// </summary>
        public decimal EventAttendanceRate { get; set; }

        /// <summary>
        /// Average number of registrations per event
        /// </summary>
        public decimal AverageRegistrationsPerEvent { get; set; }

        /// <summary>
        /// Average event rating
        /// </summary>
        public decimal AverageEventRating { get; set; }
    }
}







