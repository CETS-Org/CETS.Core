using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Data;

public partial class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService = null!;
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    #region DbSet
    public virtual DbSet<ACAD_AcademicRequest> ACAD_AcademicRequests { get; set; }

    public virtual DbSet<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; }

    public virtual DbSet<ACAD_Assignment> ACAD_Assignments { get; set; }

    public virtual DbSet<ACAD_WeeklyFeedback> ACAD_WeeklyFeedbacks { get; set; }

    public virtual DbSet<ACAD_Attendance> ACAD_Attendances { get; set; }

    public virtual DbSet<ACAD_Class> ACAD_Classes { get; set; }

    public virtual DbSet<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; }

    public virtual DbSet<ACAD_ClassReservation> ACAD_ClassReservations { get; set; }

    public virtual DbSet<ACAD_Course> ACAD_Courses { get; set; }
    public virtual DbSet<ACAD_CourseBenefit> ACAD_CourseBenefits { get; set; }
    public virtual DbSet<ACAD_CourseRequirement> ACAD_CourseRequirements { get; set; }

    public virtual DbSet<ACAD_CourseSkill> ACAD_CourseSkills { get; set; }

    public virtual DbSet<ACAD_CourseCategory> ACAD_CourseCategories { get; set; }

    public virtual DbSet<ACAD_CoursePackage> ACAD_CoursePackages { get; set; }

    public virtual DbSet<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; }

    public virtual DbSet<ACAD_CourseSchedule> ACAD_CourseSchedules { get; set; }

    public virtual DbSet<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignments { get; set; }

    public virtual DbSet<ACAD_Enrollment> ACAD_Enrollments { get; set; }

    public virtual DbSet<ACAD_LearningMaterial> ACAD_LearningMaterials { get; set; }

    public virtual DbSet<ACAD_Submission> ACAD_Submissions { get; set; }

    public virtual DbSet<ACAD_PlacementTest> ACAD_PlacementTests { get; set; }

    public virtual DbSet<ACAD_PlacementQuestion> ACAD_PlacementQuestions { get; set; }

    public virtual DbSet<ACAD_Syllabus> ACAD_Syllabi { get; set; }

    public virtual DbSet<ACAD_SyllabusItem> ACAD_SyllabusItems { get; set; }

    public virtual DbSet<COM_Conversation> COM_Conversations { get; set; }

    public virtual DbSet<COM_Feedback> COM_Feedbacks { get; set; }

    public virtual DbSet<COM_FeedbackRecord> COM_FeedbackRecords { get; set; }

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

    public virtual DbSet<ACAD_ReservationItem> ACAD_ReservationItems { get; set; }

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
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IDN_Account>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AccountID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.HasIndex(e => e.Email, "UQ_IDN_Accounts_Email").IsUnique();

            entity.HasOne(d => d.AccountStatus).WithMany(p => p.IDN_Accounts).HasConstraintName("FK_IDN_Accounts_AccountStatus");
            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation).HasConstraintName("FK_IDN_Accounts_Updated");
        });

        modelBuilder.Entity<ACAD_AcademicRequest>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RequestID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AcademicRequestStatus).WithMany(p => p.ACAD_AcademicRequestAcademicRequestStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Status");

            entity.HasOne(d => d.Priority).WithMany(p => p.ACAD_AcademicRequestPriorities)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ACAD_AcReq_Priority");

            entity.HasOne(d => d.FromClass).WithMany(p => p.ACAD_AcademicRequestFromClasses).HasConstraintName("FK_ACAD_AcReq_FromClass");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.ACAD_AcademicRequests).HasConstraintName("FK_ACAD_AcReq_Processed");

            entity.HasOne(d => d.RequestType).WithMany(p => p.ACAD_AcademicRequestRequestTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Type");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_AcademicRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReq_Student");

            entity.HasOne(d => d.ToClass).WithMany(p => p.ACAD_AcademicRequestToClasses).HasConstraintName("FK_ACAD_AcReq_ToClass");

            entity.HasOne(d => d.ClassMeeting).WithMany().HasForeignKey(d => d.ClassMeetingID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ACAD_AcReq_ClassMeeting");

            entity.HasOne(d => d.FromSlot).WithMany(p => p.ACAD_AcademicRequestFromSlots)
                .HasForeignKey(d => d.FromSlotID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_AcReq_FromSlot");

            entity.HasOne(d => d.ToSlot).WithMany(p => p.ACAD_AcademicRequestToSlots)
                .HasForeignKey(d => d.ToSlotID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_AcReq_ToSlot");

            entity.HasOne(d => d.NewRoom).WithMany()
                .HasForeignKey(d => d.NewRoomID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ACAD_AcReq_NewRoom");
        });

        modelBuilder.Entity<ACAD_AcademicRequestHistory>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("HistoryID").ValueGeneratedNever();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_AcademicRequestHistories).HasConstraintName("FK_ACAD_AcReqHist_ChangedBy");

            entity.HasOne(d => d.Request).WithMany(p => p.ACAD_AcademicRequestHistories).HasConstraintName("FK_ACAD_AcReqHist_Request");

            entity.HasOne(d => d.Status).WithMany(p => p.ACAD_AcademicRequestHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_AcReqHist_AcademicRequestStatus");
        });

        modelBuilder.Entity<ACAD_Assignment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AssignmentID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.QuestionUrl).HasMaxLength(500);
            entity.Property(e => e.AssignmentType).HasMaxLength(50).HasDefaultValue("homework");

            entity.HasOne(d => d.ClassMeeting).WithMany(p => p.ACAD_Assignments).HasConstraintName("FK_ACAD_Assignments_ClassMeeting");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_AssignmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Assignments_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_AssignmentUpdatedByNavigations).HasConstraintName("FK_ACAD_Assignments_Updated");

            entity.HasOne(d => d.Skill).WithMany(p => p.ACAD_Assignments)
                .HasForeignKey(d => d.SkillID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ACAD_Assignment_Skills");
        });

        modelBuilder.Entity<ACAD_Attendance>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AttendanceID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AttendanceStatus).WithMany(p => p.ACAD_Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Attendance_Status");

            entity.HasOne(d => d.CheckedByNavigation).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_CheckedBy");

            entity.HasOne(d => d.Meeting).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_Meeting");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Attendance_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_Attendances).HasConstraintName("FK_ACAD_Attendance_Updated");
        });

        modelBuilder.Entity<ACAD_Class>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ClassID").ValueGeneratedNever();
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
            entity.Property(e => e.Id).HasColumnName("MeetingID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsStudy).HasDefaultValue(false);

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

            entity.HasOne(d => d.Slot).WithMany(p => p.ACAD_ClassMeetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ClassMeetings_Slot");
        });

        modelBuilder.Entity<ACAD_ClassReservation>(entity =>
        {

            entity.Property(e => e.Id).HasColumnName("ClassReservationID").ValueGeneratedNever();

            entity.HasOne(d => d.CoursePackage)
                  .WithMany(p => p.ACAD_ClassReservations)
                  .HasForeignKey(d => d.CoursePackageID)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_ACAD_ClassReservations_Package");

           
            entity.HasOne(d => d.Student)
                  .WithMany(p => p.ACAD_ClassReservations)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_ACAD_ClassReservations_Student");
            entity.HasOne(d => d.ReservationStatus).WithMany(p => p.ACAD_ClassReservations)
                .HasConstraintName("FK_ACAD_ACAD_ClassReservations_ReservationStatus");
        });

        modelBuilder.Entity<ACAD_WeeklyFeedback>(entity =>
        {
            entity.ToTable("ACAD_WeeklyFeedback");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.ClassID, e.StudentID, e.WeekNumber })
                  .IsUnique();

            entity.Property(e => e.Participation)
                  .HasMaxLength(2000)
                  .IsRequired();

            entity.Property(e => e.AssignmentQuality)
                  .HasMaxLength(2000)
                  .IsRequired();

            entity.Property(e => e.SkillProgress)
                  .HasMaxLength(2000)
                  .IsRequired();

            entity.Property(e => e.NextStep)
                  .HasMaxLength(2000);

            entity.Property(e => e.CustomNote)
                  .HasMaxLength(2000);

            entity.Property(e => e.Status)
                  .HasDefaultValue(1);

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("(sysutcdatetime())");


            entity.HasOne(d => d.Teacher)
                 .WithMany(p => p.ACAD_WeeklyFeedbacks)
                 .HasForeignKey(d => d.TeacherID)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("FK_WeeklyFeedback_Teacher");

            entity.HasOne(d => d.Class)
                  .WithMany(p => p.ACAD_WeeklyFeedbacks)
                  .HasForeignKey(d => d.ClassID)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_WeeklyFeedback_Class");

            entity.HasOne(d => d.Student)
                  .WithMany(p => p.ACAD_WeeklyFeedbacks)
                  .HasForeignKey(d => d.StudentID)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_WeeklyFeedback_Student");

            entity.HasOne(d => d.ClassMeeting)
                  .WithMany(p => p.ACAD_WeeklyFeedbacks)
                  .HasForeignKey(d => d.ClassMeetingID)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_WeeklyFeedback_ClassMeeting");

            entity.HasIndex(e => e.TeacherID);
            entity.HasIndex(e => e.StudentID);
            entity.HasIndex(e => new { e.ClassID, e.WeekNumber });

        });


        modelBuilder.Entity<ACAD_Course>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CourseID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.StandardPrice).HasColumnType("decimal(18, 2)");

            // Configure AverageRating as computed column using scalar function
            entity.Property(e => e.AverageRating)
                .HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.ACAD_Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Category");

            entity.HasOne(d => d.CourseFormat).WithMany(p => p.ACAD_CourseCourseFormats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Format");

            entity.HasOne(d => d.CourseLevel).WithMany(p => p.ACAD_CourseCourseLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Courses_Level");

            entity.Property(e => e.CourseObjective)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!) ?? new())
                .HasColumnType("nvarchar(max)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CourseCreatedByNavigations).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ACAD_Courses_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CourseUpdatedByNavigations).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ACAD_Courses_Updated");
        });

        modelBuilder.Entity<ACAD_CourseCategory>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CategoryID").ValueGeneratedNever();
        });

        modelBuilder.Entity<ACAD_CoursePackage>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("PackageID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CoursePackageCreatedByNavigations).HasConstraintName("FK_ACAD_CoursePackages_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CoursePackageUpdatedByNavigations).HasConstraintName("FK_ACAD_CoursePackages_Updated");
        });

        modelBuilder.Entity<ACAD_CoursePackageItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("PackageItemID").ValueGeneratedNever();

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CoursePackageItems).HasConstraintName("FK_ACAD_CoursePackageItems_Course");

            entity.HasOne(d => d.Package).WithMany(p => p.ACAD_CoursePackageItems).HasConstraintName("FK_ACAD_CoursePackageItems_Package");
        });

        modelBuilder.Entity<ACAD_CourseTeacherAssignment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AssignmentID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_CourseTeacherAssignments).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_CourseTeacherAssignmentCreatedByNavigations).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Created");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ACAD_CourseTeacherAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_CourseTeacherAssignments_Teacher");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_CourseTeacherAssignmentUpdatedByNavigations).HasConstraintName("FK_ACAD_CourseTeacherAssignments_Updated");
        });

        modelBuilder.Entity<ACAD_Enrollment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("EnrollmentID").ValueGeneratedNever();
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

            // 1-1 relationship with Invoice (nullable - enrollments can exist without invoices initially)
            entity.HasOne(d => d.Invoice).WithOne(p => p.ACAD_Enrollment)
                .HasForeignKey<ACAD_Enrollment>(d => d.InvoiceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Enrollments_Invoice")
                .IsRequired(false);
        });

        modelBuilder.Entity<ACAD_LearningMaterial>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("MaterialID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ClassMeeting)
                .WithMany()
                .HasForeignKey(d => d.ClassMeetingID)
                .HasConstraintName("FK_ACAD_LearningMaterials_ClassMeeting");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_LearningMaterialCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_LearningMaterials_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_LearningMaterialUpdatedByNavigations)
                .HasConstraintName("FK_ACAD_LearningMaterials_Updated");
        });

        modelBuilder.Entity<ACAD_Submission>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("SubmissionID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsAiScore).HasDefaultValue(false);
            entity.Property<string?>("Title").HasMaxLength(255).HasColumnName("Title");

            entity.HasOne(d => d.Assignment).WithMany(p => p.ACAD_Submissions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ACAD_Submissions_Assignment");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SubmissionCreatedByNavigations).HasConstraintName("FK_ACAD_Submissions_Created");

            entity.HasOne(d => d.Student).WithMany(p => p.ACAD_Submissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_Submissions_Student");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SubmissionUpdatedByNavigations).HasConstraintName("FK_ACAD_Submissions_Updated");
        });

        modelBuilder.Entity<ACAD_PlacementTest>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("PlacementTestID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_PlacementTests_Created");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany()
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_PlacementTests_Updated");
        });

        modelBuilder.Entity<ACAD_PlacementQuestion>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("PlacementQuestionID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Skill)
                .WithMany()
                .HasForeignKey(d => d.SkillTypeID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_PlacementQuestions_SkillType");

            entity.HasOne(d => d.QuestionType)
                .WithMany()
                .HasForeignKey(d => d.QuestionTypeID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_PlacementQuestions_QuestionTypeID");

            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_PlacementQuestions_Created");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany()
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ACAD_PlacementQuestions_Updated");
        });

        modelBuilder.Entity<ACAD_Syllabus>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("SyllabusID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_Syllabi).HasConstraintName("FK_ACAD_Syllabi_Course");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SyllabusCreatedByNavigations).HasConstraintName("FK_ACAD_Syllabi_Created");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SyllabusUpdatedByNavigations).HasConstraintName("FK_ACAD_Syllabi_Updated");
        });

        modelBuilder.Entity<ACAD_SyllabusItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("SyllabusItemID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Required).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ACAD_SyllabusItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_SyllabusItems_Created");

            entity.HasOne(d => d.Syllabus).WithMany(p => p.ACAD_SyllabusItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_SyllabusItems_Syllabus");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ACAD_SyllabusItemUpdatedByNavigations).HasConstraintName("FK_ACAD_SyllabusItems_Updated");
        });

        modelBuilder.Entity<COM_Conversation>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ConversationID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Recipient).WithMany(p => p.COM_ConversationRecipients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Conversation_Recipient");

            entity.HasOne(d => d.Sender).WithMany(p => p.COM_ConversationSenders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Conversation_Sender");
        });

        modelBuilder.Entity<COM_Feedback>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("FeedbackID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            // Disable OUTPUT clause for tables with triggers
            entity.ToTable(tb => tb.HasTrigger("TR_COM_Feedback_Audit"));

            entity.HasOne(d => d.Course).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_COM_Feedback_Course");

            entity.HasOne(d => d.FeedbackType).WithMany(p => p.COM_Feedbacks)
                .HasConstraintName("FK_COM_Feedback_FeedbackTypeID");

            entity.HasOne(d => d.Submitter).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COM_Feedback_Submitter");

            entity.HasOne(d => d.Teacher).WithMany(p => p.COM_Feedbacks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_COM_Feedback_Teacher");

            // Configure string length for feedback fields
            entity.Property(e => e.ContentClarity).HasMaxLength(50);
            entity.Property(e => e.CourseRelevance).HasMaxLength(50);
            entity.Property(e => e.MaterialsQuality).HasMaxLength(50);
            entity.Property(e => e.TeachingEffectiveness).HasMaxLength(50);
            entity.Property(e => e.CommunicationSkills).HasMaxLength(50);
            entity.Property(e => e.TeacherSupportiveness).HasMaxLength(50);
        });

        modelBuilder.Entity<COM_FeedbackRecord>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("FeedbackRecordID").ValueGeneratedNever();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.COM_FeedbackRecords).HasConstraintName("FK_COM_FeedbackRecord_Created");
        });

     
        modelBuilder.Entity<CORE_LookUp>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("LookUpID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LookUpType).WithMany(p => p.CORE_LookUps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CORE_LookUps_Type");
        });

        modelBuilder.Entity<CORE_LookUpType>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("LookUpTypeID").ValueGeneratedNever();
        });

        modelBuilder.Entity<EVT_Event>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("EventID").ValueGeneratedNever();

            entity.HasOne(d => d.EventType).WithMany(p => p.EVT_Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RPT_Events_EventType");
        });

        modelBuilder.Entity<EVT_EventFeedback>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("EventFeedbackID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithMany(p => p.EVT_EventFeedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVT_EventFeedback_Account");

            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventFeedbacks).HasConstraintName("FK_EVT_EventFeedback_Event");
        });

        modelBuilder.Entity<EVT_EventRegistration>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RegistrationID").ValueGeneratedNever();
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithMany(p => p.EVT_EventRegistrations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_EVT_Regs_Account");

            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventRegistrations).HasConstraintName("FK_EVT_Regs_Event");
        });

        modelBuilder.Entity<FAC_Room>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RoomID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FAC_RoomCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAC_Rooms_Created");

            entity.HasOne(d => d.RoomType).WithMany(p => p.FAC_Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAC_Rooms_RoomType");

            entity.HasOne(d => d.RoomStatus).WithMany(p => p.FAC_RoomRoomStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAC_Rooms_RoomStatus");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FAC_RoomUpdatedByNavigations).HasConstraintName("FK_FAC_Rooms_Updated");
        });

        modelBuilder.Entity<FIN_Invoice>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("InvoiceID").ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(CONVERT([date],sysutcdatetime()))");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.IsInstallment).HasDefaultValue(false);

            entity.HasOne(d => d.InvoiceStatus).WithMany(p => p.FIN_InvoiceInvoiceStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Invoices_Status");


            entity.HasOne(d => d.Student).WithMany(p => p.FIN_Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Invoices_Student");
        });

        modelBuilder.Entity<FIN_InvoiceItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("InvoiceItemID").ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Course).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Course");

            entity.HasOne(d => d.CoursePackage).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Package");

            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Invoice");

            entity.HasOne(d => d.Promotion).WithMany(p => p.FIN_InvoiceItems).HasConstraintName("FK_FIN_InvoiceItems_Promotion");
        });

        modelBuilder.Entity<FIN_Payment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("PaymentID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.GatewayStatus).IsUnicode(false).HasMaxLength(30);

            entity.HasOne(d => d.Gateway).WithMany(p => p.FIN_PaymentGateways).HasConstraintName("FK_FIN_Payments_Gateway");

            entity.HasOne(d => d.Invoice).WithMany(p => p.FIN_Payments).HasConstraintName("FK_FIN_Payments_Invoice");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.FIN_PaymentPaymentMethods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Payments_Method");
        });

        modelBuilder.Entity<FIN_PaymentRefund>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RefundID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Gateway).WithMany(p => p.FIN_PaymentRefunds).HasConstraintName("FK_FIN_PaymentRefunds_Gateway");
            entity.Property(e => e.GatewayStatus).IsUnicode(false).HasMaxLength(30);
            entity.HasOne(d => d.Payment).WithMany(p => p.FIN_PaymentRefunds).HasConstraintName("FK_FIN_PaymentRefunds_Payment");
        });

        modelBuilder.Entity<FIN_PaymentWebhook>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("WebhookID").ValueGeneratedNever();
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
            entity.Property(e => e.Id).HasColumnName("PromotionID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PercentOff).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.AmountOff).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FIN_PromotionCreatedByNavigations).HasConstraintName("FK_FIN_Promotions_Created");

            entity.HasOne(d => d.PromotionType).WithMany(p => p.FIN_Promotions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FIN_Promotions_Type");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FIN_PromotionUpdatedByNavigations).HasConstraintName("FK_FIN_Promotions_Updated");
        });

        modelBuilder.Entity<ACAD_ReservationItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ReservationItemID").ValueGeneratedNever();

            // 1-1 relationship with Invoice
            entity.HasOne(d => d.Invoice).WithOne(p => p.ACAD_ReservationItem)
                .HasForeignKey<ACAD_ReservationItem>(d => d.InvoiceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ReservationItems_Invoice");

            entity.HasOne(d => d.Course).WithMany(p => p.ACAD_ReservationItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACAD_ReservationItems_Course");

            entity.HasOne(d => d.PlanType).WithMany(p => p.ACAD_ReservationItems)
                .HasConstraintName("FK_ACAD_ReservationItems_PlanType");
            entity.HasOne(d => d.ClassReservation).WithMany(p => p.ACAD_ReservationItems)
                .HasConstraintName("FK_ACAD_ReservationItems_ClassReservation");
        });

        modelBuilder.Entity<HR_Contract>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ContractID").ValueGeneratedNever();
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
            entity.Property(e => e.Id).HasColumnName("AvailabilityID").ValueGeneratedNever();

            entity.HasOne(d => d.Teacher).WithMany(p => p.HR_TeacherAvailabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HR_TeacherAvailability_Teacher");

            entity.HasIndex(e => new { e.TeacherID, e.TeachDay, e.TimeSlotID })
                .IsUnique()
                .HasDatabaseName("UQ_HR_TeacherAvailabilities_Teacher_Day_Slot");
            
        });

        modelBuilder.Entity<IDN_AccountRole>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AccountRoleID").HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Account).WithMany(p => p.IDN_AccountRoles).HasConstraintName("FK_IDN_AccountRoles_Account");

            entity.HasOne(d => d.Role).WithMany(p => p.IDN_AccountRoles).HasConstraintName("FK_IDN_AccountRoles_Role");
        });

        modelBuilder.Entity<IDN_Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RoleID").ValueGeneratedNever();
        });

        modelBuilder.Entity<IDN_Student>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AccountID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithOne(p => p.IDN_StudentAccount)
                .HasForeignKey<IDN_Student>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IDN_Student_Account");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_StudentUpdatedByNavigations).HasConstraintName("FK_IDN_Student_Update");
        });

        modelBuilder.Entity<IDN_Teacher>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AccountID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithOne(p => p.IDN_TeacherAccount)
                .HasForeignKey<IDN_Teacher>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IDN_Teachers_Account");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_TeacherUpdatedByNavigations).HasConstraintName("FK_IDN_Teachers_Update");
        });

        modelBuilder.Entity<IDN_TeacherCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__IDN_Teac__2C58F9EC154F9DC2");

            entity.Property(e => e.Id).HasColumnName("CredentialID").HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.CredentialType).WithMany(p => p.IDN_TeacherCredentials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherCredentials_CredentialType");

            entity.HasOne(d => d.Teacher).WithMany(p => p.IDN_TeacherCredentials).HasConstraintName("FK_TeacherCredentials_Teacher");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IDN_TeacherCredentials).HasConstraintName("FK_TeacherCredentials_Update");
        });

        modelBuilder.Entity<RPT_Report>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ReportID").ValueGeneratedNever();
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


        modelBuilder.Entity<ACAD_CourseBenefit>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CourseBenefitID").ValueGeneratedNever();

            // 2. Configure the relationship to ACAD_Course.
            entity.HasOne(cb => cb.Course)
             .WithMany(c => c.ACAD_CourseBenefits)
             .HasForeignKey(cb => cb.CourseID)
             .OnDelete(DeleteBehavior.Cascade);

            // 3. Configure the relationship to CORE_LookUp.
            entity.HasOne(cb => cb.Benefit)
                  .WithMany(l => l.ACAD_CourseBenefits)
                  .HasForeignKey(cb => cb.BenefitID)
                  .OnDelete(DeleteBehavior.Restrict); // You shouldn't be able to delete a lookup if it's in use.

        });

        modelBuilder.Entity<ACAD_CourseSkill>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CourseSkillID").ValueGeneratedNever();

            // 2. Configure the relationship to ACAD_Course.
            entity.HasOne(cb => cb.Course)
             .WithMany(c => c.ACAD_CourseSkills)
             .HasForeignKey(cb => cb.CourseID)
             .OnDelete(DeleteBehavior.Cascade);

            // 3. Configure the relationship to CORE_LookUp.
            entity.HasOne(cb => cb.Skill)
                  .WithMany(l => l.ACAD_CourseSkills)
                  .HasForeignKey(cb => cb.SkillID)
                  .OnDelete(DeleteBehavior.Restrict); // You shouldn't be able to delete a lookup if it's in use.

        });

        modelBuilder.Entity<ACAD_CourseRequirement>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CourseRequirementID").ValueGeneratedNever();

            entity.HasOne(cb => cb.Course)
                .WithMany(c => c.ACAD_CourseRequirements)
                .HasForeignKey(cb => cb.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cb => cb.Requirement)
               .WithMany(l => l.ACAD_CourseRequirements)
               .HasForeignKey(cb => cb.RequirementID)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ACAD_CourseSchedule>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("CourseScheduleID").ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
       

            entity.HasOne(d => d.Course)
                .WithMany(p => p.ACAD_CourseSchedules)
                .HasForeignKey(d => d.CourseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ACAD_CourseSchedules_Course");

            entity.HasOne(d => d.TimeSlot)
                .WithMany(p => p.ACAD_CourseSchedules)
                .HasForeignKey(d => d.TimeSlotID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ACAD_CourseSchedules_TimeSlot");
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
            entity.HasIndex(e => e.Email, "UQ_IDN_Accounts_Email").IsUnique();
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
            entity.HasIndex(e => e.TeacherCode, "UQ_IDN_Teachers_Code").IsUnique();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("IDN_Teachers", t => t.HasCheckConstraint("CK_IDN_Teachers_YearsExp", "[YearsExperience] >= 0"));
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
            entity.ToTable("ACAD_SyllabusItems", t =>
            {
                t.HasCheckConstraint("CK_ACAD_SyllabusItems_Session", "[SessionNumber] >= 1");
                t.HasCheckConstraint("CK_ACAD_SyllabusItems_Slots", "[TotalSlots] > 0");
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

        modelBuilder.HasSequence<int>("SeqClass");
        modelBuilder.Entity<ACAD_Class>(entity =>
        {
            entity.Property(x => x.ClassNum)
          .HasDefaultValueSql("NEXT VALUE FOR [SeqClass]");

            entity.Property(x => x.ClassName)
                  .HasMaxLength(50)
                  .HasComputedColumnSql(
                      "('CLS' + RIGHT('0000' + CONVERT(varchar(4), [ClassNum]), 4))",
                      stored: true);

            entity.HasIndex(x => x.ClassName)
                  .IsUnique()
                  .HasDatabaseName("UX_ACAD_Classes_ClassName");
            entity.Property(e => e.EnrolledCount).HasDefaultValue(0);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.ToTable("ACAD_Classes", t =>
            {
                t.HasCheckConstraint("CK_ACAD_Classes_Capacity", "[Capacity] > 0");
                t.HasCheckConstraint("CK_ACAD_Classes_EnrolledCount", "[EnrolledCount] >= 0");
                t.HasCheckConstraint("CK_ACAD_Classes_Dates", "[EndDate] >= [StartDate]");
            });
        });

        modelBuilder.Entity<ACAD_ClassMeeting>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsStudy).HasDefaultValue(false);
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
            entity.ToTable("EVT_EventRegistrations", t =>
            {
                t.HasCheckConstraint("CK_EVT_Regs_UserOrEmail", "[AccountID] IS NOT NULL OR ([Email] IS NOT NULL AND LEN(LTRIM(RTRIM([Email]))) > 0)");
                t.HasCheckConstraint("CK_EVT_Regs_CheckTimes", "[CheckOutAt] IS NULL OR ([CheckInAt] IS NOT NULL AND [CheckOutAt] >= [CheckInAt])");
            });
        });

        modelBuilder.Entity<EVT_EventFeedback>(entity =>
        {
            entity.HasOne(d => d.Event).WithMany(p => p.EVT_EventFeedbacks).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_EVT_EventFeedback_Event");
            entity.ToTable("EVT_EventFeedback", t => t.HasCheckConstraint("CK_EVT_EventFeedback_Rating", "[Rating] BETWEEN 1 AND 5"));
        });

        modelBuilder.HasSequence<int>("SeqStudent");
        modelBuilder.Entity<IDN_Student>(e =>
        {
            e.Property(x => x.StudentNumber)
             .HasDefaultValueSql("NEXT VALUE FOR [SeqStudent]");

            e.Property(x => x.StudentCode)
             .HasComputedColumnSql("('STU'+RIGHT('000000'+CONVERT(varchar(6), [StudentNumber]), 6))", stored: true);

            e.HasIndex(x => x.StudentCode)
             .IsUnique()
             .HasDatabaseName("UX_IDN_Students_StudentCode");
        });

        modelBuilder.HasSequence<int>("SeqTeacher");
        modelBuilder.Entity<IDN_Teacher>(e =>
        {
            e.Property<int>("TeacherNumber")
             .HasDefaultValueSql("NEXT VALUE FOR [SeqTeacher]");

            e.Property(x => x.TeacherCode)
             .HasComputedColumnSql("('TCH'+RIGHT('000000'+CONVERT(varchar(6), [TeacherNumber]), 6))", stored: true);

            e.HasIndex(x => x.TeacherCode)
             .IsUnique()
             .HasDatabaseName("UX_IDN_Teachers_TeacherCode");
        });

        modelBuilder.HasSequence<int>("InvoiceSequence", schema: "dbo")
               .StartsAt(1)
               .IncrementsBy(1);
        modelBuilder.Entity<FIN_Invoice>(entity =>
        {
            entity.Property(e => e.InvoiceSequence)
             .IsRequired()
             .HasDefaultValueSql("NEXT VALUE FOR dbo.InvoiceSequence");

            entity.Property(e => e.InvoiceNumber)
              .HasColumnName("InvoiceNumber")
              .HasComputedColumnSql("'INV-' + CONVERT(VARCHAR(4), YEAR(GETDATE())) + RIGHT('0000000' + CONVERT(VARCHAR(7), [InvoiceSequence]), 7)", stored: false);
        });
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditing();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void HandleAuditing()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IHasCreationTime || e.Entity is IHasModificationTime || e.Entity is IHasCreator || e.Entity is IHasModifier)
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        if (!entries.Any())
        {
            return;
        }

        //TODO: Replace with actual system user ID or a dedicated service account ID.
        //Temporary hardcoded admin ID for system processes when no user is logged in.
        var currentUserId = _currentUserService.UserId ?? /* Guid.Empty*/ Guid.Parse("2782B49E-CDCC-4A1E-BAAE-E74DE022D657");
        var now = DateTime.Now;

        foreach (var entry in entries)
        {
            // --- Handle Created properties for new entities ---
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is IHasCreationTime creationTimeEntity)
                {
                    creationTimeEntity.CreatedAt = now;
                }

                if (entry.Entity is IHasCreator creatorEntity)
                {
                    creatorEntity.CreatedBy = currentUserId;
                }

                continue;
            }

            // --- Handle Modified properties for existing entities ---
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IHasModificationTime modificationTimeEntity)
                {
                    modificationTimeEntity.UpdatedAt = now;
                }

                if (entry.Entity is IHasModifier modifierEntity)
                {
                    modifierEntity.UpdatedBy = currentUserId;
                }
            }
        }
    }
}