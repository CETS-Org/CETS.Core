using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_ReservationItem.Responses;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassReservation.Responses
{
    public class ClassReservationResponse
    {
        public Guid Id { get; set; }
        public Guid StudentID { get; set; }
        public Guid? CoursePackageID { get; set; }
        public string? PackageCode { get; set; } = string.Empty;
        public string? PackageName { get; set; } = string.Empty;
        public string? PackageImageUrl { get; set; }
        public decimal? TotalPrice { get; set; }

        public string? Description { get; set; }
        public string? ReservationStatus { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        

    }
}
