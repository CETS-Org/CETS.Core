using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class CORE_LookUp : AuditedEntity
{
    public Guid LookUpTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequestAcademicRequestStatuses { get; set; } = new List<ACAD_AcademicRequest>();

    public virtual ICollection<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; } = new List<ACAD_AcademicRequestHistory>();

    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequestRequestTypes { get; set; } = new List<ACAD_AcademicRequest>();

    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    public virtual ICollection<ACAD_Class> ACAD_ClassClassStatuses { get; set; } = new List<ACAD_Class>();

    public virtual ICollection<ACAD_Class> ACAD_ClassCourseFormats { get; set; } = new List<ACAD_Class>();

    public virtual ICollection<ACAD_Course> ACAD_CourseCourseFormats { get; set; } = new List<ACAD_Course>();

    public virtual ICollection<ACAD_Course> ACAD_CourseCourseLevels { get; set; } = new List<ACAD_Course>();

    public virtual ICollection<ACAD_Enrollment> ACAD_Enrollments { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<COM_Feedback> COM_Feedbacks { get; set; } = new List<COM_Feedback>();

    public virtual ICollection<EVT_Event> EVT_Events { get; set; } = new List<EVT_Event>();

    public virtual ICollection<FAC_Room> FAC_Rooms { get; set; } = new List<FAC_Room>();

    public virtual ICollection<FIN_Invoice> FIN_InvoiceInvoiceStatuses { get; set; } = new List<FIN_Invoice>();

    public virtual ICollection<FIN_Payment> FIN_PaymentGateways { get; set; } = new List<FIN_Payment>();

    public virtual ICollection<FIN_Payment> FIN_PaymentPaymentMethods { get; set; } = new List<FIN_Payment>();

    public virtual ICollection<FIN_PaymentRefund> FIN_PaymentRefunds { get; set; } = new List<FIN_PaymentRefund>();

    public virtual ICollection<FIN_PaymentWebhook> FIN_PaymentWebhooks { get; set; } = new List<FIN_PaymentWebhook>();

    public virtual ICollection<FIN_Promotion> FIN_Promotions { get; set; } = new List<FIN_Promotion>();

    public virtual ICollection<ACAD_ReservationItem> FIN_ReservationItems { get; set; } = new List<ACAD_ReservationItem>();

    public virtual ICollection<HR_Contract> HR_Contracts { get; set; } = new List<HR_Contract>();

    public virtual ICollection<IDN_Account> IDN_Accounts { get; set; } = new List<IDN_Account>();

    public virtual ICollection<IDN_TeacherCredential> IDN_TeacherCredentials { get; set; } = new List<IDN_TeacherCredential>();

    public virtual ICollection<RPT_Report> RPT_ReportReportStatuses { get; set; } = new List<RPT_Report>();

    public virtual ICollection<RPT_Report> RPT_ReportReportTypes { get; set; } = new List<RPT_Report>();

    public virtual ICollection<ACAD_CourseRequirement> ACAD_CourseRequirements { get; set; } = new List<ACAD_CourseRequirement>();
    public virtual ICollection<ACAD_CourseBenefit> ACAD_CourseBenefits { get; set; } = new List<ACAD_CourseBenefit>();
    public virtual ICollection<ACAD_CourseSkill> ACAD_CourseSkills { get; set; } = new List<ACAD_CourseSkill>();
    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; } = new List<ACAD_ClassMeeting>();
    public virtual ICollection<ACAD_CourseSchedule> ACAD_CourseSchedules { get; set; } = new List<ACAD_CourseSchedule>();

    [ForeignKey(nameof(LookUpTypeID))]
    public virtual CORE_LookUpType LookUpType { get; set; } = null!;

}
