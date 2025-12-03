using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class IDN_Account : EntityBase, IHasCreationTime, IHasModificationTime, IHasModifier
{
    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(100)]
    public string? CID { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    public string? AvatarUrl { get; set; }

    [StringLength(255)]
    public string? Password { get; set; }

    public Guid? AccountStatusID { get; set; }

    public bool IsVerified { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? VerifiedCode { get; set; }

    [Precision(0)]
    public DateTime? VerifiedCodeExpiresAt { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; } = new List<ACAD_AcademicRequestHistory>();

    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequests { get; set; } = new List<ACAD_AcademicRequest>();

    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    public virtual ICollection<ACAD_Class> ACAD_ClassCreatedByNavigations { get; set; } = new List<ACAD_Class>();

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetingCreatedByNavigations { get; set; } = new List<ACAD_ClassMeeting>();

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetingUpdatedByNavigations { get; set; } = new List<ACAD_ClassMeeting>();

    public virtual ICollection<ACAD_Class> ACAD_ClassUpdatedByNavigations { get; set; } = new List<ACAD_Class>();

    public virtual ICollection<ACAD_Course> ACAD_CourseCreatedByNavigations { get; set; } = new List<ACAD_Course>();

    public virtual ICollection<ACAD_CoursePackage> ACAD_CoursePackageCreatedByNavigations { get; set; } = new List<ACAD_CoursePackage>();

    public virtual ICollection<ACAD_CoursePackage> ACAD_CoursePackageUpdatedByNavigations { get; set; } = new List<ACAD_CoursePackage>();

    public virtual ICollection<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignmentCreatedByNavigations { get; set; } = new List<ACAD_CourseTeacherAssignment>();

    public virtual ICollection<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignmentUpdatedByNavigations { get; set; } = new List<ACAD_CourseTeacherAssignment>();

    public virtual ICollection<ACAD_Course> ACAD_CourseUpdatedByNavigations { get; set; } = new List<ACAD_Course>();

    public virtual ICollection<ACAD_Enrollment> ACAD_EnrollmentCreatedByNavigations { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_Enrollment> ACAD_EnrollmentUpdatedByNavigations { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_LearningMaterial> ACAD_LearningMaterialCreatedByNavigations { get; set; } = new List<ACAD_LearningMaterial>();


    public virtual ICollection<ACAD_LearningMaterial> ACAD_LearningMaterialUpdatedByNavigations { get; set; } = new List<ACAD_LearningMaterial>();


    public virtual ICollection<ACAD_Submission> ACAD_SubmissionCreatedByNavigations { get; set; } = new List<ACAD_Submission>();


    public virtual ICollection<ACAD_Submission> ACAD_SubmissionUpdatedByNavigations { get; set; } = new List<ACAD_Submission>();


    public virtual ICollection<ACAD_Syllabus> ACAD_SyllabusCreatedByNavigations { get; set; } = new List<ACAD_Syllabus>();


    public virtual ICollection<ACAD_SyllabusItem> ACAD_SyllabusItemCreatedByNavigations { get; set; } = new List<ACAD_SyllabusItem>();


    public virtual ICollection<ACAD_SyllabusItem> ACAD_SyllabusItemUpdatedByNavigations { get; set; } = new List<ACAD_SyllabusItem>();


    public virtual ICollection<ACAD_Syllabus> ACAD_SyllabusUpdatedByNavigations { get; set; } = new List<ACAD_Syllabus>();

    public virtual CORE_LookUp? AccountStatus { get; set; }

    public virtual ICollection<COM_Conversation> COM_ConversationRecipients { get; set; } = new List<COM_Conversation>();

    public virtual ICollection<COM_Conversation> COM_ConversationSenders { get; set; } = new List<COM_Conversation>();


    public virtual ICollection<COM_FeedbackRecord> COM_FeedbackRecords { get; set; } = new List<COM_FeedbackRecord>();


    public virtual ICollection<EVT_EventFeedback> EVT_EventFeedbacks { get; set; } = new List<EVT_EventFeedback>();


    public virtual ICollection<EVT_EventRegistration> EVT_EventRegistrations { get; set; } = new List<EVT_EventRegistration>();


    public virtual ICollection<FAC_Room> FAC_RoomCreatedByNavigations { get; set; } = new List<FAC_Room>();


    public virtual ICollection<FAC_Room> FAC_RoomUpdatedByNavigations { get; set; } = new List<FAC_Room>();


    public virtual ICollection<FIN_Promotion> FIN_PromotionCreatedByNavigations { get; set; } = new List<FIN_Promotion>();


    public virtual ICollection<FIN_Promotion> FIN_PromotionUpdatedByNavigations { get; set; } = new List<FIN_Promotion>();


    public virtual ICollection<HR_Contract> HR_ContractCreatedByNavigations { get; set; } = new List<HR_Contract>();


    public virtual ICollection<HR_Contract> HR_ContractUpdatedByNavigations { get; set; } = new List<HR_Contract>();


    public virtual ICollection<IDN_AccountRole> IDN_AccountRoles { get; set; } = new List<IDN_AccountRole>();


    public virtual IDN_Student? IDN_StudentAccount { get; set; }


    public virtual ICollection<IDN_Student> IDN_StudentUpdatedByNavigations { get; set; } = new List<IDN_Student>();


    public virtual IDN_Teacher? IDN_TeacherAccount { get; set; }


    public virtual ICollection<IDN_TeacherCredential> IDN_TeacherCredentials { get; set; } = new List<IDN_TeacherCredential>();


    public virtual ICollection<IDN_Teacher> IDN_TeacherUpdatedByNavigations { get; set; } = new List<IDN_Teacher>();


    public virtual ICollection<IDN_Account> InverseUpdatedByNavigation { get; set; } = new List<IDN_Account>();

    public virtual ICollection<RPT_Report> RPT_ReportResolvedByNavigations { get; set; } = new List<RPT_Report>();

    public virtual ICollection<RPT_Report> RPT_ReportSubmittedByNavigations { get; set; } = new List<RPT_Report>();

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
