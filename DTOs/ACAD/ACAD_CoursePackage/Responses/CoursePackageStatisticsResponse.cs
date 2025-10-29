namespace DTOs.ACAD.ACAD_CoursePackage.Responses
{
    public class CoursePackageStatisticsResponse
    {
        public int TotalPackages { get; set; }
        public int ActivePackages { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PackagesSold { get; set; }
    }
}

