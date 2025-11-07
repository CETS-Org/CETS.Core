namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Center Performance - Operational Efficiency Metrics
    /// Tổng hợp công suất trung tâm, tỉ lệ sử dụng phòng học, lớp mở/đóng
    /// </summary>
    public class CenterPerformanceResponse
    {
        // Room & Facility Utilization
        /// <summary>
        /// Total number of rooms/facilities
        /// </summary>
        public int TotalRooms { get; set; }

        /// <summary>
        /// Number of active rooms available for use
        /// </summary>
        public int ActiveRooms { get; set; }

        /// <summary>
        /// Room occupancy rate - percentage of room capacity being used
        /// Formula: (Used room-hours / Total available room-hours) * 100
        /// </summary>
        public decimal RoomOccupancyRate { get; set; }

        /// <summary>
        /// Average room utilization across all rooms (%)
        /// </summary>
        public decimal AverageRoomUtilization { get; set; }

        // Class Operations
        /// <summary>
        /// Total classes opened this month
        /// </summary>
        public int ClassesOpenedThisMonth { get; set; }

        /// <summary>
        /// Total classes closed/completed this month
        /// </summary>
        public int ClassesClosedThisMonth { get; set; }

        /// <summary>
        /// Currently active/ongoing classes
        /// </summary>
        public int ActiveClassesCount { get; set; }

        // Operational Efficiency
        /// <summary>
        /// Overall center utilization rate (%)
        /// Combines class fill rate, room occupancy, and teacher utilization
        /// </summary>
        public decimal OverallUtilizationRate { get; set; }

        /// <summary>
        /// Average class fill rate across all classes (%)
        /// </summary>
        public decimal AverageClassFillRate { get; set; }

        /// <summary>
        /// Operational efficiency score (0-100)
        /// Composite metric of all efficiency indicators
        /// </summary>
        public decimal OperationalEfficiencyScore { get; set; }

        // Capacity Metrics
        /// <summary>
        /// Total student capacity across all active classes
        /// </summary>
        public int TotalStudentCapacity { get; set; }

        /// <summary>
        /// Currently enrolled students count
        /// </summary>
        public int CurrentEnrollmentCount { get; set; }

        /// <summary>
        /// Remaining available capacity
        /// </summary>
        public int AvailableCapacity { get; set; }

        /// <summary>
        /// Capacity utilization percentage
        /// </summary>
        public decimal CapacityUtilizationRate { get; set; }
    }
}





