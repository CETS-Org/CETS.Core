using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    #region DbSet
    public virtual DbSet<ACAD_AcademicRequest> ACAD_AcademicRequests { get; set; }

    public virtual DbSet<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; }

    public virtual DbSet<ACAD_Assignment> ACAD_Assignments { get; set; }

    public virtual DbSet<ACAD_Attendance> ACAD_Attendances { get; set; }

    public virtual DbSet<ACAD_Class> ACAD_Classes { get; set; }

    public virtual DbSet<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; }

    public virtual DbSet<ACAD_ClassReservation> ACAD_ClassReservations { get; set; }

    public virtual DbSet<ACAD_Course> ACAD_Courses { get; set; }

    public virtual DbSet<ACAD_CourseCategory> ACAD_CourseCategories { get; set; }

    public virtual DbSet<ACAD_CoursePackage> ACAD_CoursePackages { get; set; }

    public virtual DbSet<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; }

    public virtual DbSet<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignments { get; set; }

    public virtual DbSet<ACAD_Enrollment> ACAD_Enrollments { get; set; }

    public virtual DbSet<ACAD_LearningMaterial> ACAD_LearningMaterials { get; set; }

    public virtual DbSet<ACAD_Submission> ACAD_Submissions { get; set; }

    public virtual DbSet<ACAD_Syllabus> ACAD_Syllabi { get; set; }

    public virtual DbSet<ACAD_SyllabusItem> ACAD_SyllabusItems { get; set; }

    public virtual DbSet<COM_Conversation> COM_Conversations { get; set; }

    public virtual DbSet<COM_Feedback> COM_Feedbacks { get; set; }

    public virtual DbSet<COM_FeedbackRecord> COM_FeedbackRecords { get; set; }

    public virtual DbSet<COM_Notification> COM_Notifications { get; set; }

    public virtual DbSet<CORE_LookUp> CORE_LookUps { get; set; }

    public virtual DbSet<CORE_LookUpType> CORE_LookUpTypes { get; set; }

    public virtual DbSet<EVT_Event> EVT_Events { get; set; }

    public virtual DbSet<EVT_EventFeedback> EVT_EventFeedbacks { get; set; }

    public virtual DbSet<EVT_EventRegistration> EVT_EventRegistrations { get; set; }

    public virtual DbSet<FAC_Room> FAC_Rooms { get; set; }

    public virtual DbSet<FIN_Invoice> FIN_Invoices { get; set; }

    public virtual DbSet<FIN_InvoiceItem> FIN_InvoiceItems { get; set; }

    public virtual DbSet<FIN_Payment> FIN_Payments { get; set; }

    public virtual DbSet<FIN_PaymentRefund> FIN_PaymentRefunds { get; set; }

    public virtual DbSet<FIN_PaymentWebhook> FIN_PaymentWebhooks { get; set; }

    public virtual DbSet<FIN_Promotion> FIN_Promotions { get; set; }

    public virtual DbSet<HR_Contract> HR_Contracts { get; set; }

    public virtual DbSet<HR_TeacherAvailability> HR_TeacherAvailabilities { get; set; }

    public virtual DbSet<IDN_Account> IDN_Accounts { get; set; }

    public virtual DbSet<IDN_AccountRole> IDN_AccountRoles { get; set; }

    public virtual DbSet<IDN_Role> IDN_Roles { get; set; }

    public virtual DbSet<IDN_Student> IDN_Students { get; set; }

    public virtual DbSet<IDN_Teacher> IDN_Teachers { get; set; }

    public virtual DbSet<IDN_TeacherCredential> IDN_TeacherCredentials { get; set; }

    public virtual DbSet<RPT_Report> RPT_Reports { get; set; }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ACAD_AcademicRequest>(entity =>
        {
            entity.Property(e => e.RequestID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AcademicRequestStatus).WithMany(p => p.ACAD_AcademicRequestAcademicRequestStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Status");

            entity.HasOne(d => d.FromClass).WithMany(p => p.ACAD_AcademicRequestFromClasses).HasConstraintName("FK_ACAD_AcReq_FromClass");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.ACAD_AcademicRequests).HasConstraintName("FK_ACAD_AcReq_Processed");

            entity.HasOne(d => d.RequestType).WithMany(p => p.ACAD_AcademicRequestRequestTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Type");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_AcademicRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Student");

            entity.HasOne(d => d.ToClass).WithMany(p => p.ACAD_AcademicRequestToClasses).HasConstraintName("FK_ACAD_AcReq_ToClass");
        });

        modelBuilder.Entity<ACAD_AcademicRequestHistory>(entity =>
        {
            entity.Property(e => e.HistoryID).ValueGeneratedNever();
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.ACAD_AcademicRequestHistories).HasConstraintName("FK_ACAD_AcReqHist_ChangedBy");

            entity.HasOne(d => d.Request).WithMany(p => p.ACAD_AcademicRequestHistories).HasConstraintName("FK_ACAD_AcReqHist_Request");

            entity.HasOne(d => d.Status).WithMany(p => p.ACAD_AcademicRequestHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReqHist_AcademicRequestStatus");
        });

        modelBuilder.Entity<ACAD_Assignment>(entity =>
        {
            entity.Property(e => e.AssignmentID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ClassMeeting).WithMany(p => p.ACAD_Assignments).HasConstraintName("FK_ACAD_Assignments_ClassMeeting");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_AssignmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Assignments_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_AssignmentUpdatedByNavigations).HasConstraintName("FK_ACAD_Assignments_Updated");
        });

        modelBuilder.Entity<ACAD_Attendance>(entity =>
        {
            entity.Property(e => e.AttendanceID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AttendanceStatus).WithMany(p => p.ACAD_Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Attendance_Status");

            entity.HasOne(d => d.CheckByNavigation).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_Created");

            entity.HasOne(d => d.Meeting).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_Meeting");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Attendance_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_Updated");
        });

        modelBuilder.Entity<ACAD_Class>(entity =>
        {
            entity.Property(e => e.ClassID).ValueGeneratedNever();
            entity.Property(e => e.Capacity).HasDefaultValue(30);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ClassStatus).WithMany(p => p.ACAD_ClassClassStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Classes_Status");

            entity.HasOne(d => d.CourseFormat).WithMany(p => p.ACAD_ClassCourseFormats).HasConstraintName("FK_ACAD_Classes_Format");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_ClassCreatedByNavigations).HasConstraintName("FK_ACAD_Classes_Created");

            entity.HasOne(d => d.TeacherAssignment).WithMany(p => p.ACAD_Classes).HasConstraintName("FK_ACAD_Classes_Assignment");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_ClassUpdatedByNavigations).HasConstraintName("FK_ACAD_Classes_Updated");
        });

        modelBuilder.Entity<ACAD_ClassMeeting>(entity =>
        {
            entity.Property(e => e.MeetingID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Class).WithMany(p => p.ACAD_ClassMeetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ClassMeetings_Class");

            entity.HasOne(d => d.CoveredTopic).WithMany(p => p.ACAD_ClassMeetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ClassMeetings_CoveredTopic");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_ClassMeetingCreatedByNavigations).HasConstraintName("FK_ACAD_ClassMeetings_Created");

            entity.HasOne(d => d.Room).WithMany(p => p.ACAD_ClassMeetings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ACAD_ClassMeetings_Room");

            entity.HasOne(d => d.TeacherAssignment).WithMany(p => p.ACAD_ClassMeetings).HasConstraintName("FK_ACAD_ClassMeetings_Assignment");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_ClassMeetingUpdatedByNavigations).HasConstraintName("FK_ACAD_ClassMeetings_Updated");
        });

        modelBuilder.Entity<ACAD_ClassReservation>(entity =>
        {
            entity.Property(e => e.ReservationID).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Class).WithMany(p => p.ACAD_ClassReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ClassReservations_Class");

            entity.HasOne(d => d.Invoice).WithMany(p => p.ACAD_ClassReservations).HasConstraintName("FK_ACAD_ClassReservations_Invoice");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_ClassReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ClassReservations_Student");
        });

        modelBuilder.Entity<ACAD_Course>(entity =>
        {
            entity.Property(e => e.CourseID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Category).WithMany(p => p.ACAD_Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Category");

            entity.HasOne(d => d.CourseFormat).WithMany(p => p.ACAD_CourseCourseFormats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Format");

            entity.HasOne(d => d.CourseLevel).WithMany(p => p.ACAD_CourseCourseLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Level");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CourseCreatedByNavigations).HasConstraintName("FK_ACAD_Courses_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CourseUpdatedByNavigations).HasConstraintName("FK_ACAD_Courses_Updated");
        });

        modelBuilder.Entity<ACAD_CourseCategory>(entity =>
        {
            entity.Property(e => e.CategoryID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ACAD_CoursePackage>(entity =>
        {
            entity.Property(e => e.PackageID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CoursePackageCreatedByNavigations).HasConstraintName("FK_ACAD_CoursePackages_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CoursePackageUpdatedByNavigations).HasConstraintName("FK_ACAD_CoursePackages_Updated");
        });

        modelBuilder.Entity<ACAD_CoursePackageItem>(entity =>
        {
            entity.Property(e => e.PackageItemID).ValueGeneratedNever();

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CoursePackageItems).HasConstraintName("FK_ACAD_CoursePackageItems_Course");

            entity.HasOne(d => d.Package).WithMany(p => p.ACAD_CoursePackageItems).HasConstraintName("FK_ACAD_CoursePackageItems_Package");
        });

        modelBuilder.Entity<ACAD_CourseTeacherAssignment>(entity =>
        {
            entity.Property(e => e.AssignmentID).ValueGeneratedNever();
            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CourseTeacherAssignments).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CourseTeacherAssignmentCreatedByNavigations).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Created");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ACAD_CourseTeacherAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_CourseTeacherAssignments_Teacher");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CourseTeacherAssignmentUpdatedByNavigations).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Updated");
        });

        modelBuilder.Entity<ACAD_Enrollment>(entity =>
        {
            entity.Property(e => e.EnrollmentID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Class).WithMany(p => p.ACAD_Enrollments).HasConstraintName("FK_ACAD_Enrollments_Class");

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Enrollments_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_EnrollmentCreatedByNavigations).HasConstraintName("FK_ACAD_Enrollments_Created");

            entity.HasOne(d => d.EnrollmentStatus).WithMany(p => p.ACAD_Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Enrollments_Status");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Enrollments_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_EnrollmentUpdatedByNavigations).HasConstraintName("FK_ACAD_Enrollments_Updated");
        });

        modelBuilder.Entity<ACAD_LearningMaterial>(entity =>
        {
            entity.Property(e => e.MaterialID).ValueGeneratedNever();
            entity.Property(e => e.UploadDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Class).WithMany(p => p.ACAD_LearningMaterials).HasConstraintName("FK_ACAD_LearningMaterials_Class");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_LearningMaterialCreatedByNavigations).HasConstraintName("FK_ACAD_LearningMaterials_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_LearningMaterialUpdatedByNavigations).HasConstraintName("FK_ACAD_LearningMaterials_Updated");

            entity.HasOne(d => d.Uploader).WithMany(p => p.ACAD_LearningMaterialUploaders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_LearningMaterials_Uploader");
        });

        modelBuilder.Entity<ACAD_Submission>(entity =>
        {
            entity.Property(e => e.SubmissionID).ValueGeneratedNever();
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Assignment).WithMany(p => p.ACAD_Submissions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ACAD_Submissions_Assignment");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SubmissionCreatedByNavigations).HasConstraintName("FK_ACAD_Submissions_Created");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_Submissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Submissions_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SubmissionUpdatedByNavigations).HasConstraintName("FK_ACAD_Submissions_Updated");
        });

        modelBuilder.Entity<ACAD_Syllabus>(entity =>
        {
            entity.Property(e => e.SyllabusID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_Syllabi).HasConstraintName("FK_ACAD_Syllabi_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SyllabusCreatedByNavigations).HasConstraintName("FK_ACAD_Syllabi_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SyllabusUpdatedByNavigations).HasConstraintName("FK_ACAD_Syllabi_Updated");
        });

        modelBuilder.Entity<ACAD_SyllabusItem>(entity =>
        {
            entity.Property(e => e.SyllabusItemID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Required).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SyllabusItemCreatedByNavigations).HasConstraintName("FK_ACAD_SyllabusItems_Created");

            entity.HasOne(d => d.Syllabus).WithMany(p => p.ACAD_SyllabusItems).HasConstraintName("FK_ACAD_SyllabusItems_Syllabus");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SyllabusItemUpdatedByNavigations).HasConstraintName("FK_ACAD_SyllabusItems_Updated");
        });

        modelBuilder.Entity<COM_Conversation>(entity =>
        {
            entity.Property(e => e.ConversationID).ValueGeneratedNever();
            entity.Property(e => e.StartAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Recipient).WithMany(p => p.COM_ConversationRecipients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Conversation_Recipient");

            entity.HasOne(d => d.Sender).WithMany(p => p.COM_ConversationSenders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Conversation_Sender");
        });

        modelBuilder.Entity<COM_Feedback>(entity =>
        {
            entity.Property(e => e.FeedbackID).ValueGeneratedNever();
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_COM_Feedback_Course");

            entity.HasOne(d => d.FeedbackType).WithMany(p => p.COM_Feedbacks).HasConstraintName("FK_COM_Feedback_FeedbackTypeID");

            entity.HasOne(d => d.Submitter).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Feedback_Submitter");

            entity.HasOne(d => d.Teacher).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_COM_Feedback_Teacher");
        });

        modelBuilder.Entity<COM_FeedbackRecord>(entity =>
        {
            entity.Property(e => e.FeedbackRecordID).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.COM_FeedbackRecords).HasConstraintName("FK_COM_FeedbackRecord_Created");
        });

        modelBuilder.Entity<COM_Notification>(entity =>
        {
            entity.Property(e => e.NotificationID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<CORE_LookUp>(entity =>
        {
            entity.Property(e => e.LookUpID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LookUpType).WithMany(p => p.CORE_LookUps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORE_LookUps_Type");
        });

        modelBuilder.Entity<CORE_LookUpType>(entity =>
        {
            entity.Property(e => e.LookUpTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<EVT_Event>(entity =>
        {
            entity.Property(e => e.EventID).ValueGeneratedNever();

            entity.HasOne(d => d.EventType).WithMany(p => p.EVT_Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RPT_Events_EventType");
        });

        modelBuilder.Entity<EVT_EventFeedback>(entity =>
        {
            entity.Property(e => e.EventFeedbackID).ValueGeneratedNever();
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithMany(p => p.EVT_EventFeedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVT_EventFeedback_Account");

            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventFeedbacks).HasConstraintName("FK_EVT_EventFeedback_Event");
        });

        modelBuilder.Entity<EVT_EventRegistration>(entity =>
        {
            entity.Property(e => e.RegistrationID).ValueGeneratedNever();
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithMany(p => p.EVT_EventRegistrations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_EVT_Regs_Account");

            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventRegistrations).HasConstraintName("FK_EVT_Regs_Event");
        });

        modelBuilder.Entity<FAC_Room>(entity =>
        {
            entity.Property(e => e.RoomID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FAC_RoomCreatedByNavigations).HasConstraintName("FK_FAC_Rooms_Created");

            entity.HasOne(d => d.RoomType).WithMany(p => p.FAC_Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAC_Rooms_RoomType");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FAC_RoomUpdatedByNavigations).HasConstraintName("FK_FAC_Rooms_Updated");
        });

        modelBuilder.Entity<FIN_Invoice>(entity =>
        {
            entity.HasIndex(e => new { e.SeriesID, e.Sequence }, "IX_FIN_Invoices_SeriesSeq_Filtered")
                .IsUnique()
                .HasFilter("([SeriesID] IS NOT NULL AND [Sequence] IS NOT NULL)");

            entity.Property(e => e.InvoiceID).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(CONVERT([date],sysutcdatetime()))");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.InvoiceStatus).WithMany(p => p.FIN_InvoiceInvoiceStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Invoices_Status");

            entity.HasOne(d => d.PlanType).WithMany(p => p.FIN_InvoicePlanTypes).HasConstraintName("FK_FIN_Invoices_PlanType");

            entity.HasOne(d => d.Student).WithMany(p => p.FIN_Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Invoices_Student");
        });

        modelBuilder.Entity<FIN_InvoiceItem>(entity =>
        {
            entity.Property(e => e.InvoiceItemID).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Course).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Course");

            entity.HasOne(d => d.CoursePackage).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Package");

            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Invoice");

            entity.HasOne(d => d.Promotion).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Promotion");
        });

        modelBuilder.Entity<FIN_Payment>(entity =>
        {
            entity.Property(e => e.PaymentID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Gateway).WithMany(p => p.FIN_PaymentGateways).HasConstraintName("FK_FIN_Payments_Gateway");

            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_Payments).HasConstraintName("FK_FIN_Payments_Invoice");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.FIN_PaymentPaymentMethods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Payments_Method");
        });

        modelBuilder.Entity<FIN_PaymentRefund>(entity =>
        {
            entity.Property(e => e.RefundID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Gateway).WithMany(p => p.FIN_PaymentRefunds).HasConstraintName("FK_FIN_PaymentRefunds_Gateway");

            entity.HasOne(d => d.Payment).WithMany(p => p.FIN_PaymentRefunds).HasConstraintName("FK_FIN_PaymentRefunds_Payment");
        });

        modelBuilder.Entity<FIN_PaymentWebhook>(entity =>
        {
            entity.Property(e => e.WebhookID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ReceivedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Gateway).WithMany(p => p.FIN_PaymentWebhooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_PaymentWebhooks_Gateway");

            entity.HasOne(d => d.Payment).WithMany(p => p.FIN_PaymentWebhooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_PaymentWebhooks_Payment");
        });

        modelBuilder.Entity<FIN_Promotion>(entity =>
        {
            entity.Property(e => e.PromotionID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FIN_PromotionCreatedByNavigations).HasConstraintName("FK_FIN_Promotions_Created");

            entity.HasOne(d => d.PromotionType).WithMany(p => p.FIN_Promotions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Promotions_Type");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FIN_PromotionUpdatedByNavigations).HasConstraintName("FK_FIN_Promotions_Updated");
        });

        modelBuilder.Entity<HR_Contract>(entity =>
        {
            entity.Property(e => e.ContractID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FileHash).IsFixedLength();

            entity.HasOne(d => d.ContractStatus).WithMany(p => p.HR_Contracts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HR_Contracts_Status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HR_ContractCreatedByNavigations).HasConstraintName("FK_HR_Contracts_Created");

            entity.HasOne(d => d.Teacher).WithMany(p => p.HR_Contracts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HR_Contracts_Teacher");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HR_ContractUpdatedByNavigations).HasConstraintName("FK_HR_Contracts_Updated");
        });

        modelBuilder.Entity<HR_TeacherAvailability>(entity =>
        {
            entity.Property(e => e.AvailabilityID).ValueGeneratedNever();

            entity.HasOne(d => d.Teacher).WithMany(p => p.HR_TeacherAvailabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HR_TeacherAvailability_Teacher");
        });

        modelBuilder.Entity<IDN_Account>(entity =>
        {
            entity.Property(e => e.AccountID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AccountStatus).WithMany(p => p.IDN_Accounts).HasConstraintName("FK_IDN_Accounts_AccountStatus");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation).HasConstraintName("FK_IDN_Accounts_Updated");
        });

        modelBuilder.Entity<IDN_AccountRole>(entity =>
        {
            entity.Property(e => e.AccountRoleID).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Account).WithMany(p => p.IDN_AccountRoles).HasConstraintName("FK_IDN_AccountRoles_Account");

            entity.HasOne(d => d.Role).WithMany(p => p.IDN_AccountRoles).HasConstraintName("FK_IDN_AccountRoles_Role");
        });

        modelBuilder.Entity<IDN_Role>(entity =>
        {
            entity.Property(e => e.RoleID).ValueGeneratedNever();
        });

        modelBuilder.Entity<IDN_Student>(entity =>
        {
            entity.Property(e => e.AccountID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithOne(p => p.IDN_StudentAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IDN_Student_Account");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_StudentUpdatedByNavigations).HasConstraintName("FK_IDN_Student_Update");
        });

        modelBuilder.Entity<IDN_Teacher>(entity =>
        {
            entity.Property(e => e.AccountID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithOne(p => p.IDN_TeacherAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IDN_Teacher_Account");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_TeacherUpdatedByNavigations).HasConstraintName("FK_IDN_Teacher_Update");
        });

        modelBuilder.Entity<IDN_TeacherCredential>(entity =>
        {
            entity.HasKey(e => e.CredentialID).HasName("PK__IDN_Teac__2C58F9EC154F9DC2");

            entity.Property(e => e.CredentialID).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.CredentialType).WithMany(p => p.IDN_TeacherCredentials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherCredentials_CredentialType");

            entity.HasOne(d => d.Teacher).WithMany(p => p.IDN_TeacherCredentials).HasConstraintName("FK_TeacherCredentials_Teacher");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_TeacherCredentials).HasConstraintName("FK_TeacherCredentials_Update");
        });

        modelBuilder.Entity<RPT_Report>(entity =>
        {
            entity.Property(e => e.ReportID).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ReportStatus).WithMany(p => p.RPT_ReportReportStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RPT_Reports_Status");

            entity.HasOne(d => d.ReportType).WithMany(p => p.RPT_ReportReportTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RPT_Reports_Type");

            entity.HasOne(d => d.ResolvedByNavigation).WithMany(p => p.RPT_ReportResolvedByNavigations).HasConstraintName("FK_RPT_Reports_Resolved");

            entity.HasOne(d => d.SubmittedByNavigation).WithMany(p => p.RPT_ReportSubmittedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RPT_Reports_Submitter");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // --- CORE ---
        modelBuilder.Entity<CORE_LookUpType>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_CORE_LookUpTypes_Code").IsUnique();
        });

        modelBuilder.Entity<CORE_LookUp>(entity =>
        {
            entity.HasIndex(e => new { e.LookUpTypeID, e.Code }, "UQ_CORE_LookUps_Type_Code").IsUnique();
        });

        // --- IDENTITY / ACCOUNTS ---
        modelBuilder.Entity<IDN_Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName, "UQ_IDN_Roles_RoleName").IsUnique();
        });

        modelBuilder.Entity<IDN_Account>(entity =>
        {
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<IDN_AccountRole>(entity =>
        {
            entity.HasIndex(e => new { e.AccountID, e.RoleID }, "UQ_IDN_AccountRoles").IsUnique();

            entity.HasOne(d => d.Account).WithMany(p => p.IDN_AccountRoles).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_IDN_AccountRoles_Account");
            entity.HasOne(d => d.Role).WithMany(p => p.IDN_AccountRoles).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_IDN_AccountRoles_Role");
        });

        modelBuilder.Entity<IDN_Student>(entity =>
        {
            entity.HasIndex(e => e.StudentCode, "UQ_IDN_Student_Code").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<IDN_Teacher>(entity =>
        {
            entity.HasIndex(e => e.TeacherCode, "UQ_IDN_Teacher_Code").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("IDN_Teacher", t => t.HasCheckConstraint("CK_IDN_Teacher_YearsExp", "[YearsExperience] >= 0"));
        });

        modelBuilder.Entity<IDN_TeacherCredential>(entity =>
        {
            entity.HasOne(d => d.Teacher).WithMany(p => p.IDN_TeacherCredentials).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_TeacherCredentials_Teacher");
        });

        // --- FACILITIES ---
        modelBuilder.Entity<FAC_Room>(entity =>
        {
            entity.ToTable("FAC_Rooms", t => t.HasCheckConstraint("CK_FAC_Rooms_Capacity", "[Capacity] > 0"));
        });

        // --- ACADEMICS ---
        modelBuilder.Entity<ACAD_CourseCategory>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_ACAD_CourseCategory_Code").IsUnique();
        });

        modelBuilder.Entity<ACAD_Course>(entity =>
        {
            entity.HasIndex(e => e.CourseCode, "UQ_ACAD_Courses_CourseCode").IsUnique();
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_CourseTeacherAssignment>(entity =>
        {
            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CourseTeacherAssignments).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Course");
        });

        modelBuilder.Entity<ACAD_Syllabus>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_Syllabi).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_Syllabi_Course");
        });

        modelBuilder.Entity<ACAD_SyllabusItem>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("ACAD_SyllabusItems", t => {
                t.HasCheckConstraint("CK_ACAD_SyllabusItems_Session", "[SessionNumber] >= 1");
                t.HasCheckConstraint("CK_ACAD_SyllabusItems_Minutes", "[EstimatedMinutes] > 0");
            });
            entity.HasOne(d => d.Syllabus).WithMany(p => p.ACAD_SyllabusItems).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_SyllabusItems_Syllabus");
        });

        modelBuilder.Entity<ACAD_CoursePackage>(entity =>
        {
            entity.HasIndex(e => e.PackageCode, "UQ_ACAD_CoursePackages_Code").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_CoursePackageItem>(entity =>
        {
            entity.HasIndex(e => new { e.PackageID, e.Sequence }, "UQ_ACAD_CoursePackageItems_Package_Sequence").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.HasOne(d => d.Package).WithMany(p => p.ACAD_CoursePackageItems).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_CoursePackageItems_Package");
            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CoursePackageItems).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_CoursePackageItems_Course");
        });

        modelBuilder.Entity<ACAD_Class>(entity =>
        {
            entity.Property(e => e.EnrolledCount).HasDefaultValue(0);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("ACAD_Classes", t => {
                t.HasCheckConstraint("CK_ACAD_Classes_Capacity", "[Capacity] > 0");
                t.HasCheckConstraint("CK_ACAD_Classes_EnrolledCount", "[EnrolledCount] >= 0");
                t.HasCheckConstraint("CK_ACAD_Classes_Dates", "[EndDate] >= [StartDate]");
            });
        });

        modelBuilder.Entity<ACAD_ClassMeeting>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("ACAD_ClassMeetings", t => t.HasCheckConstraint("CK_ACAD_ClassMeetings_Times", "[EndsAt] > [StartsAt]"));
        });

        modelBuilder.Entity<ACAD_ClassReservation>(entity =>
        {
            entity.HasIndex(e => new { e.ClassID, e.StudentID }, "UQ_ACAD_ClassReservations").IsUnique();
        });

        modelBuilder.Entity<ACAD_Enrollment>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_Attendance>(entity =>
        {
            entity.HasIndex(e => new { e.MeetingID, e.StudentID }, "UQ_ACAD_Attendance").IsUnique();
            entity.HasOne(d => d.Meeting).WithMany(p => p.ACAD_Attendances).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_Attendance_Meeting");
        });

        modelBuilder.Entity<ACAD_LearningMaterial>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_Assignment>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_Submission>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<ACAD_AcademicRequestHistory>(entity =>
        {
            entity.HasOne(d => d.Request).WithMany(p => p.ACAD_AcademicRequestHistories).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ACAD_AcReqHist_Request");
        });

        // --- FINANCE ---
        modelBuilder.Entity<FIN_Promotion>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_FIN_Promotions_Code").IsUnique();
            entity.ToTable("FIN_Promotions", t => t.HasCheckConstraint("CK_FIN_Promotions_Amount", "([PercentOff] IS NOT NULL AND [AmountOff] IS NULL AND [PercentOff] BETWEEN 0 AND 100) OR ([AmountOff] IS NOT NULL AND [PercentOff] IS NULL AND [AmountOff] >= 0)"));
        });

        modelBuilder.Entity<FIN_Invoice>(entity =>
        {
            entity.HasIndex(e => e.InvoiceNumber, "UQ_FIN_Invoices_Number").IsUnique();
            entity.Property(e => e.Subtotal).HasDefaultValue(0);
            entity.Property(e => e.TaxAmount).HasDefaultValue(0);
            entity.Property(e => e.TotalAmount).HasDefaultValue(0);
        });

        modelBuilder.Entity<FIN_InvoiceItem>(entity =>
        {
            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_InvoiceItems).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_FIN_InvoiceItems_Invoice");
            entity.ToTable("FIN_InvoiceItems", t => t.HasCheckConstraint("CK_FIN_InvoiceItems_XOR", "([CourseID] IS NOT NULL AND [CoursePackageID] IS NULL) OR ([CourseID] IS NULL AND [CoursePackageID] IS NOT NULL)"));
        });

        modelBuilder.Entity<FIN_Payment>(entity =>
        {
            entity.HasIndex(e => new { e.GatewayID, e.TransactionID }, "UQ_FIN_Payments_GatewayTxn").IsUnique();
            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_Payments).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_FIN_Payments_Invoice");
        });

        modelBuilder.Entity<FIN_PaymentRefund>(entity =>
        {
            entity.HasOne(d => d.Payment).WithMany(p => p.FIN_PaymentRefunds).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_FIN_PaymentRefunds_Payment");
        });

        modelBuilder.Entity<FIN_PaymentWebhook>(entity =>
        {
            entity.HasIndex(e => new { e.GatewayID, e.EventId }, "UQ_FIN_PaymentWebhooks").IsUnique();
        });

        // --- COMMUNICATIONS ---
        modelBuilder.Entity<COM_Notification>(entity =>
        {
            entity.Property(e => e.IsPush).HasDefaultValue(false);
        });

        modelBuilder.Entity<COM_Feedback>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("COM_Feedback", t => t.HasCheckConstraint("CK_COM_Feedback_Rating", "[Rating] IS NULL OR ([Rating] BETWEEN 1 AND 5)"));
        });

        modelBuilder.Entity<COM_FeedbackRecord>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        // --- HR ---
        modelBuilder.Entity<HR_Contract>(entity =>
        {
            entity.HasIndex(e => e.ContractNumber, "UQ_HR_Contracts_Code").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("HR_Contracts", t => t.HasCheckConstraint("CK_HR_Contracts_Dates", "[ExpiredAt] IS NULL OR [ExpiredAt] >= [SignedAt]"));
        });

        // --- EVENTS ---
        modelBuilder.Entity<EVT_Event>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("EVT_Events", t => t.HasCheckConstraint("CK_EVT_Events_MaxSize", "[MaxSize] IS NULL OR [MaxSize] > 0"));
        });

        modelBuilder.Entity<EVT_EventRegistration>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventRegistrations).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_EVT_Regs_Event");
            entity.ToTable("EVT_EventRegistrations", t => {
                t.HasCheckConstraint("CK_EVT_Regs_UserOrEmail", "[AccountID] IS NOT NULL OR ([Email] IS NOT NULL AND LEN(LTRIM(RTRIM([Email]))) > 0)");
                t.HasCheckConstraint("CK_EVT_Regs_CheckTimes", "[CheckOutAt] IS NULL OR ([CheckInAt] IS NOT NULL AND [CheckOutAt] >= [CheckInAt])");
            });
        });

        modelBuilder.Entity<EVT_EventFeedback>(entity =>
        {
            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventFeedbacks).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_EVT_EventFeedback_Event");
            entity.ToTable("EVT_EventFeedback", t => t.HasCheckConstraint("CK_EVT_EventFeedback_Rating", "[Rating] BETWEEN 1 AND 5"));
        });
    }
}
