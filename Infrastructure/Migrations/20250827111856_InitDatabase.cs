using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACAD_CourseCategories",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "COM_Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    IsPush = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_Notifications", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "CORE_LookUpTypes",
                columns: table => new
                {
                    LookUpTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORE_LookUpTypes", x => x.LookUpTypeID);
                });

            migrationBuilder.CreateTable(
                name: "IDN_Roles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDN_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "CORE_LookUps",
                columns: table => new
                {
                    LookUpID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LookUpTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORE_LookUps", x => x.LookUpID);
                    table.ForeignKey(
                        name: "FK_CORE_LookUps_Type",
                        column: x => x.LookUpTypeID,
                        principalTable: "CORE_LookUpTypes",
                        principalColumn: "LookUpTypeID");
                });

            migrationBuilder.CreateTable(
                name: "EVT_Events",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    MaxSize = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVT_Events", x => x.EventID);
                    table.CheckConstraint("CK_EVT_Events_MaxSize", "[MaxSize] IS NULL OR [MaxSize] > 0");
                    table.ForeignKey(
                        name: "FK_RPT_Events_EventType",
                        column: x => x.EventTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                });

            migrationBuilder.CreateTable(
                name: "IDN_Accounts",
                columns: table => new
                {
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    CID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    VerifiedCodeExpiresAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDN_Accounts", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_IDN_Accounts_AccountStatus",
                        column: x => x.AccountStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_IDN_Accounts_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_CoursePackages",
                columns: table => new
                {
                    PackageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CoursePackages", x => x.PackageID);
                    table.ForeignKey(
                        name: "FK_ACAD_CoursePackages_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_CoursePackages_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Courses",
                columns: table => new
                {
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CourseLevelID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseFormatID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_ACAD_Courses_Category",
                        column: x => x.CategoryID,
                        principalTable: "ACAD_CourseCategories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_ACAD_Courses_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Courses_Format",
                        column: x => x.CourseFormatID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Courses_Level",
                        column: x => x.CourseLevelID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Courses_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "COM_Conversations",
                columns: table => new
                {
                    ConversationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_Conversations", x => x.ConversationID);
                    table.ForeignKey(
                        name: "FK_COM_Conversation_Recipient",
                        column: x => x.RecipientID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_COM_Conversation_Sender",
                        column: x => x.SenderID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "COM_FeedbackRecords",
                columns: table => new
                {
                    FeedbackRecordID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_FeedbackRecords", x => x.FeedbackRecordID);
                    table.ForeignKey(
                        name: "FK_COM_FeedbackRecord_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "EVT_EventFeedback",
                columns: table => new
                {
                    EventFeedbackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVT_EventFeedback", x => x.EventFeedbackID);
                    table.CheckConstraint("CK_EVT_EventFeedback_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_EVT_EventFeedback_Account",
                        column: x => x.AccountID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_EVT_EventFeedback_Event",
                        column: x => x.EventID,
                        principalTable: "EVT_Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EVT_EventRegistrations",
                columns: table => new
                {
                    RegistrationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CheckInAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CheckOutAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVT_EventRegistrations", x => x.RegistrationID);
                    table.CheckConstraint("CK_EVT_Regs_CheckTimes", "[CheckOutAt] IS NULL OR ([CheckInAt] IS NOT NULL AND [CheckOutAt] >= [CheckInAt])");
                    table.CheckConstraint("CK_EVT_Regs_UserOrEmail", "[AccountID] IS NOT NULL OR ([Email] IS NOT NULL AND LEN(LTRIM(RTRIM([Email]))) > 0)");
                    table.ForeignKey(
                        name: "FK_EVT_Regs_Account",
                        column: x => x.AccountID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EVT_Regs_Event",
                        column: x => x.EventID,
                        principalTable: "EVT_Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FAC_Rooms",
                columns: table => new
                {
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnlineMeetingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAC_Rooms", x => x.RoomID);
                    table.CheckConstraint("CK_FAC_Rooms_Capacity", "[Capacity] > 0");
                    table.ForeignKey(
                        name: "FK_FAC_Rooms_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_FAC_Rooms_RoomType",
                        column: x => x.RoomTypeId,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FAC_Rooms_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "FIN_Promotions",
                columns: table => new
                {
                    PromotionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PercentOff = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AmountOff = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_Promotions", x => x.PromotionID);
                    table.CheckConstraint("CK_FIN_Promotions_Amount", "([PercentOff] IS NOT NULL AND [AmountOff] IS NULL AND [PercentOff] BETWEEN 0 AND 100) OR ([AmountOff] IS NOT NULL AND [PercentOff] IS NULL AND [AmountOff] >= 0)");
                    table.ForeignKey(
                        name: "FK_FIN_Promotions_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_FIN_Promotions_Type",
                        column: x => x.PromotionTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_Promotions_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "IDN_AccountRoles",
                columns: table => new
                {
                    AccountRoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDN_AccountRoles", x => x.AccountRoleID);
                    table.ForeignKey(
                        name: "FK_IDN_AccountRoles_Account",
                        column: x => x.AccountID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IDN_AccountRoles_Role",
                        column: x => x.RoleID,
                        principalTable: "IDN_Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDN_Students",
                columns: table => new
                {
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GuardianName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuardianPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    School = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AcademicNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDN_Students", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_IDN_Student_Account",
                        column: x => x.AccountID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_IDN_Student_Update",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "IDN_Teachers",
                columns: table => new
                {
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    YearsExperience = table.Column<int>(type: "int", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDN_Teachers", x => x.AccountID);
                    table.CheckConstraint("CK_IDN_Teachers_YearsExp", "[YearsExperience] >= 0");
                    table.ForeignKey(
                        name: "FK_IDN_Teachers_Account",
                        column: x => x.AccountID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_IDN_Teachers_Update",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "RPT_Reports",
                columns: table => new
                {
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    ResolvedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RPT_Reports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_RPT_Reports_Resolved",
                        column: x => x.ResolvedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_RPT_Reports_Status",
                        column: x => x.ReportStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_RPT_Reports_Submitter",
                        column: x => x.SubmittedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_RPT_Reports_Type",
                        column: x => x.ReportTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_CoursePackageItems",
                columns: table => new
                {
                    PackageItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CoursePackageItems", x => x.PackageItemID);
                    table.ForeignKey(
                        name: "FK_ACAD_CoursePackageItems_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CoursePackageItems_Package",
                        column: x => x.PackageID,
                        principalTable: "ACAD_CoursePackages",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Syllabi",
                columns: table => new
                {
                    SyllabusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Syllabi", x => x.SyllabusID);
                    table.ForeignKey(
                        name: "FK_ACAD_Syllabi_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_Syllabi_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Syllabi_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "FIN_Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InvoiceStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],sysutcdatetime()))"),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(14,2)", nullable: false, defaultValue: 0m),
                    TaxAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", nullable: false, defaultValue: 0m),
                    SeriesID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    PlanTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_FIN_Invoices_IDN_Accounts_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_FIN_Invoices_PlanType",
                        column: x => x.PlanTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_Invoices_Status",
                        column: x => x.InvoiceStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_Invoices_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_CourseTeacherAssignments",
                columns: table => new
                {
                    AssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseTeacherAssignments", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseTeacherAssignments_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseTeacherAssignments_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_CourseTeacherAssignments_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_CourseTeacherAssignments_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "COM_Feedback",
                columns: table => new
                {
                    FeedbackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmitterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_Feedback", x => x.FeedbackID);
                    table.CheckConstraint("CK_COM_Feedback_Rating", "[Rating] IS NULL OR ([Rating] BETWEEN 1 AND 5)");
                    table.ForeignKey(
                        name: "FK_COM_Feedback_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_COM_Feedback_FeedbackTypeID",
                        column: x => x.FeedbackTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_COM_Feedback_Submitter",
                        column: x => x.SubmitterID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_COM_Feedback_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HR_Contracts",
                columns: table => new
                {
                    ContractID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileHash = table.Column<string>(type: "char(64)", unicode: false, fixedLength: true, maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Contracts", x => x.ContractID);
                    table.CheckConstraint("CK_HR_Contracts_Dates", "[ExpiredAt] IS NULL OR [ExpiredAt] >= [SignedAt]");
                    table.ForeignKey(
                        name: "FK_HR_Contracts_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_HR_Contracts_Status",
                        column: x => x.ContractStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_HR_Contracts_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_HR_Contracts_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "HR_TeacherAvailabilities",
                columns: table => new
                {
                    AvailabilityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Slot = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_TeacherAvailabilities", x => x.AvailabilityID);
                    table.ForeignKey(
                        name: "FK_HR_TeacherAvailability_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "IDN_TeacherCredentials",
                columns: table => new
                {
                    CredentialID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CredentialTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__IDN_Teac__2C58F9EC154F9DC2", x => x.CredentialID);
                    table.ForeignKey(
                        name: "FK_TeacherCredentials_CredentialType",
                        column: x => x.CredentialTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_TeacherCredentials_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherCredentials_Update",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_SyllabusItems",
                columns: table => new
                {
                    SyllabusItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    TopicTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EstimatedMinutes = table.Column<int>(type: "int", nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Objectives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreReadingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_SyllabusItems", x => x.SyllabusItemID);
                    table.CheckConstraint("CK_ACAD_SyllabusItems_Minutes", "[EstimatedMinutes] > 0");
                    table.CheckConstraint("CK_ACAD_SyllabusItems_Session", "[SessionNumber] >= 1");
                    table.ForeignKey(
                        name: "FK_ACAD_SyllabusItems_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_SyllabusItems_Syllabus",
                        column: x => x.SyllabusID,
                        principalTable: "ACAD_Syllabi",
                        principalColumn: "SyllabusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_SyllabusItems_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "FIN_InvoiceItems",
                columns: table => new
                {
                    InvoiceItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoursePackageID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PromotionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_InvoiceItems", x => x.InvoiceItemID);
                    table.CheckConstraint("CK_FIN_InvoiceItems_XOR", "([CourseID] IS NOT NULL AND [CoursePackageID] IS NULL) OR ([CourseID] IS NULL AND [CoursePackageID] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FIN_InvoiceItems_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK_FIN_InvoiceItems_Invoice",
                        column: x => x.InvoiceID,
                        principalTable: "FIN_Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIN_InvoiceItems_Package",
                        column: x => x.CoursePackageID,
                        principalTable: "ACAD_CoursePackages",
                        principalColumn: "PackageID");
                    table.ForeignKey(
                        name: "FK_FIN_InvoiceItems_Promotion",
                        column: x => x.PromotionID,
                        principalTable: "FIN_Promotions",
                        principalColumn: "PromotionID");
                });

            migrationBuilder.CreateTable(
                name: "FIN_Payments",
                columns: table => new
                {
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PaymentMethodID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GatewayID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GatewayStatus = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    GatewayPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_FIN_Payments_Gateway",
                        column: x => x.GatewayID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_Payments_Invoice",
                        column: x => x.InvoiceID,
                        principalTable: "FIN_Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIN_Payments_Method",
                        column: x => x.PaymentMethodID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Classes",
                columns: table => new
                {
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseFormatID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherAssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    EnrolledCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Classes", x => x.ClassID);
                    table.CheckConstraint("CK_ACAD_Classes_Capacity", "[Capacity] > 0");
                    table.CheckConstraint("CK_ACAD_Classes_Dates", "[EndDate] >= [StartDate]");
                    table.CheckConstraint("CK_ACAD_Classes_EnrolledCount", "[EnrolledCount] >= 0");
                    table.ForeignKey(
                        name: "FK_ACAD_Classes_Assignment",
                        column: x => x.TeacherAssignmentID,
                        principalTable: "ACAD_CourseTeacherAssignments",
                        principalColumn: "AssignmentID");
                    table.ForeignKey(
                        name: "FK_ACAD_Classes_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Classes_Format",
                        column: x => x.CourseFormatID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Classes_Status",
                        column: x => x.ClassStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Classes_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "FIN_PaymentRefunds",
                columns: table => new
                {
                    RefundID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GatewayID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundTxnId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GatewayStatus = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    GatewayPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_PaymentRefunds", x => x.RefundID);
                    table.ForeignKey(
                        name: "FK_FIN_PaymentRefunds_Gateway",
                        column: x => x.GatewayID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_PaymentRefunds_Payment",
                        column: x => x.PaymentID,
                        principalTable: "FIN_Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIN_PaymentWebhooks",
                columns: table => new
                {
                    WebhookID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GatewayID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_PaymentWebhooks", x => x.WebhookID);
                    table.ForeignKey(
                        name: "FK_FIN_PaymentWebhooks_Gateway",
                        column: x => x.GatewayID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_FIN_PaymentWebhooks_Payment",
                        column: x => x.PaymentID,
                        principalTable: "FIN_Payments",
                        principalColumn: "PaymentID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_AcademicRequests",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    FromClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ToClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_AcademicRequests", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_FromClass",
                        column: x => x.FromClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_Processed",
                        column: x => x.ProcessedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_Status",
                        column: x => x.AcademicRequestStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_ToClass",
                        column: x => x.ToClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReq_Type",
                        column: x => x.RequestTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_ClassMeetings",
                columns: table => new
                {
                    MeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndsAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherAssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OnlineMeetingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecordingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgressNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoveredTopicID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_ClassMeetings", x => x.MeetingID);
                    table.CheckConstraint("CK_ACAD_ClassMeetings_Times", "[EndsAt] > [StartsAt]");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_Assignment",
                        column: x => x.TeacherAssignmentID,
                        principalTable: "ACAD_CourseTeacherAssignments",
                        principalColumn: "AssignmentID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_Class",
                        column: x => x.ClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_CoveredTopic",
                        column: x => x.CoveredTopicID,
                        principalTable: "ACAD_SyllabusItems",
                        principalColumn: "SyllabusItemID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_Room",
                        column: x => x.RoomID,
                        principalTable: "FAC_Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ACAD_ClassMeetings_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_ClassReservations",
                columns: table => new
                {
                    ReservationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_ClassReservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_ACAD_ClassReservations_Class",
                        column: x => x.ClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassReservations_Invoice",
                        column: x => x.InvoiceID,
                        principalTable: "FIN_Invoices",
                        principalColumn: "InvoiceID");
                    table.ForeignKey(
                        name: "FK_ACAD_ClassReservations_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Enrollments",
                columns: table => new
                {
                    EnrollmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Enrollments", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Class",
                        column: x => x.ClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Status",
                        column: x => x.EnrollmentStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Enrollments_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_LearningMaterials",
                columns: table => new
                {
                    MaterialID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploaderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StoreUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_LearningMaterials", x => x.MaterialID);
                    table.ForeignKey(
                        name: "FK_ACAD_LearningMaterials_Class",
                        column: x => x.ClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ACAD_LearningMaterials_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_LearningMaterials_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_LearningMaterials_Uploader",
                        column: x => x.UploaderID,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_AcademicRequestHistories",
                columns: table => new
                {
                    HistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_AcademicRequestHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_ACAD_AcReqHist_AcademicRequestStatus",
                        column: x => x.StatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReqHist_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_AcReqHist_Request",
                        column: x => x.RequestID,
                        principalTable: "ACAD_AcademicRequests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Assignments",
                columns: table => new
                {
                    AssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassMeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Assignments", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_ACAD_Assignments_ClassMeeting",
                        column: x => x.ClassMeetingID,
                        principalTable: "ACAD_ClassMeetings",
                        principalColumn: "MeetingID");
                    table.ForeignKey(
                        name: "FK_ACAD_Assignments_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Assignments_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CheckBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_ACAD_Attendance_Created",
                        column: x => x.CheckBy,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Attendance_Meeting",
                        column: x => x.MeetingID,
                        principalTable: "ACAD_ClassMeetings",
                        principalColumn: "MeetingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_Attendance_Status",
                        column: x => x.AttendanceStatusID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                    table.ForeignKey(
                        name: "FK_ACAD_Attendance_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Attendance_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_Submissions",
                columns: table => new
                {
                    SubmissionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StoreUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_Submissions", x => x.SubmissionID);
                    table.ForeignKey(
                        name: "FK_ACAD_Submissions_Assignment",
                        column: x => x.AssignmentID,
                        principalTable: "ACAD_Assignments",
                        principalColumn: "AssignmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_Submissions_Created",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Submissions_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_ACAD_Submissions_Updated",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequestHistories_RequestID",
                table: "ACAD_AcademicRequestHistories",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequestHistories_StatusID",
                table: "ACAD_AcademicRequestHistories",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_AcademicRequestStatusID",
                table: "ACAD_AcademicRequests",
                column: "AcademicRequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_FromClassID",
                table: "ACAD_AcademicRequests",
                column: "FromClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ProcessedBy",
                table: "ACAD_AcademicRequests",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_RequestTypeID",
                table: "ACAD_AcademicRequests",
                column: "RequestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_StudentID",
                table: "ACAD_AcademicRequests",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ToClassID",
                table: "ACAD_AcademicRequests",
                column: "ToClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_ClassMeetingID",
                table: "ACAD_Assignments",
                column: "ClassMeetingID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_CreatedBy",
                table: "ACAD_Assignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_UpdatedBy",
                table: "ACAD_Assignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_AttendanceStatusID",
                table: "ACAD_Attendances",
                column: "AttendanceStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_CheckBy",
                table: "ACAD_Attendances",
                column: "CheckBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_StudentID",
                table: "ACAD_Attendances",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_UpdatedBy",
                table: "ACAD_Attendances",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_Attendance",
                table: "ACAD_Attendances",
                columns: new[] { "MeetingID", "StudentID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_ClassStatusID",
                table: "ACAD_Classes",
                column: "ClassStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_CourseFormatID",
                table: "ACAD_Classes",
                column: "CourseFormatID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_CreatedBy",
                table: "ACAD_Classes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_TeacherAssignmentID",
                table: "ACAD_Classes",
                column: "TeacherAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_UpdatedBy",
                table: "ACAD_Classes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_ClassID",
                table: "ACAD_ClassMeetings",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_CoveredTopicID",
                table: "ACAD_ClassMeetings",
                column: "CoveredTopicID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_CreatedBy",
                table: "ACAD_ClassMeetings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_RoomID",
                table: "ACAD_ClassMeetings",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_TeacherAssignmentID",
                table: "ACAD_ClassMeetings",
                column: "TeacherAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_UpdatedBy",
                table: "ACAD_ClassMeetings",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_InvoiceID",
                table: "ACAD_ClassReservations",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_StudentID",
                table: "ACAD_ClassReservations",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_ClassReservations",
                table: "ACAD_ClassReservations",
                columns: new[] { "ClassID", "StudentID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_CourseCategory_Code",
                table: "ACAD_CourseCategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CoursePackageItems_CourseID",
                table: "ACAD_CoursePackageItems",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_CoursePackageItems_Package_Sequence",
                table: "ACAD_CoursePackageItems",
                columns: new[] { "PackageID", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CoursePackages_CreatedBy",
                table: "ACAD_CoursePackages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CoursePackages_UpdatedBy",
                table: "ACAD_CoursePackages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_CoursePackages_Code",
                table: "ACAD_CoursePackages",
                column: "PackageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CategoryID",
                table: "ACAD_Courses",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CourseFormatID",
                table: "ACAD_Courses",
                column: "CourseFormatID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CourseLevelID",
                table: "ACAD_Courses",
                column: "CourseLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CreatedBy",
                table: "ACAD_Courses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_UpdatedBy",
                table: "ACAD_Courses",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_Courses_CourseCode",
                table: "ACAD_Courses",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CourseID",
                table: "ACAD_CourseTeacherAssignments",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CreatedBy",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_TeacherID",
                table: "ACAD_CourseTeacherAssignments",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_UpdatedBy",
                table: "ACAD_CourseTeacherAssignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_ClassID",
                table: "ACAD_Enrollments",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_CourseID",
                table: "ACAD_Enrollments",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_CreatedBy",
                table: "ACAD_Enrollments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_EnrollmentStatusID",
                table: "ACAD_Enrollments",
                column: "EnrollmentStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_StudentID",
                table: "ACAD_Enrollments",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_UpdatedBy",
                table: "ACAD_Enrollments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_ClassID",
                table: "ACAD_LearningMaterials",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_CreatedBy",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_UpdatedBy",
                table: "ACAD_LearningMaterials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_UploaderID",
                table: "ACAD_LearningMaterials",
                column: "UploaderID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_AssignmentID",
                table: "ACAD_Submissions",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_CreatedBy",
                table: "ACAD_Submissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_StudentID",
                table: "ACAD_Submissions",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_UpdatedBy",
                table: "ACAD_Submissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_CourseID",
                table: "ACAD_Syllabi",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_CreatedBy",
                table: "ACAD_Syllabi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_UpdatedBy",
                table: "ACAD_Syllabi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_CreatedBy",
                table: "ACAD_SyllabusItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_SyllabusID",
                table: "ACAD_SyllabusItems",
                column: "SyllabusID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_UpdatedBy",
                table: "ACAD_SyllabusItems",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Conversations_RecipientID",
                table: "COM_Conversations",
                column: "RecipientID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Conversations_SenderID",
                table: "COM_Conversations",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Feedback_CourseID",
                table: "COM_Feedback",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Feedback_FeedbackTypeID",
                table: "COM_Feedback",
                column: "FeedbackTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Feedback_SubmitterID",
                table: "COM_Feedback",
                column: "SubmitterID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_Feedback_TeacherID",
                table: "COM_Feedback",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_COM_FeedbackRecords_CreatedBy",
                table: "COM_FeedbackRecords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_CORE_LookUps_Type_Code",
                table: "CORE_LookUps",
                columns: new[] { "LookUpTypeID", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CORE_LookUpTypes_Code",
                table: "CORE_LookUpTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVT_EventFeedback_AccountID",
                table: "EVT_EventFeedback",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_EVT_EventFeedback_EventID",
                table: "EVT_EventFeedback",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EVT_EventRegistrations_AccountID",
                table: "EVT_EventRegistrations",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_EVT_EventRegistrations_EventID",
                table: "EVT_EventRegistrations",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EVT_Events_EventTypeID",
                table: "EVT_Events",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_CreatedBy",
                table: "FAC_Rooms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_RoomTypeId",
                table: "FAC_Rooms",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_UpdatedBy",
                table: "FAC_Rooms",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_InvoiceItems_CourseID",
                table: "FIN_InvoiceItems",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_InvoiceItems_CoursePackageID",
                table: "FIN_InvoiceItems",
                column: "CoursePackageID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_InvoiceItems_InvoiceID",
                table: "FIN_InvoiceItems",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_InvoiceItems_PromotionID",
                table: "FIN_InvoiceItems",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_CreatedBy",
                table: "FIN_Invoices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_InvoiceStatusID",
                table: "FIN_Invoices",
                column: "InvoiceStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_PlanTypeID",
                table: "FIN_Invoices",
                column: "PlanTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices",
                columns: new[] { "SeriesID", "Sequence" },
                unique: true,
                filter: "([SeriesID] IS NOT NULL AND [Sequence] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_StudentID",
                table: "FIN_Invoices",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_UpdatedBy",
                table: "FIN_Invoices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_FIN_Invoices_Number",
                table: "FIN_Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_PaymentRefunds_GatewayID",
                table: "FIN_PaymentRefunds",
                column: "GatewayID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_PaymentRefunds_PaymentID",
                table: "FIN_PaymentRefunds",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Payments_InvoiceID",
                table: "FIN_Payments",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Payments_PaymentMethodID",
                table: "FIN_Payments",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "UQ_FIN_Payments_GatewayTxn",
                table: "FIN_Payments",
                columns: new[] { "GatewayID", "TransactionID" },
                unique: true,
                filter: "[GatewayID] IS NOT NULL AND [TransactionID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_PaymentWebhooks_PaymentID",
                table: "FIN_PaymentWebhooks",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "UQ_FIN_PaymentWebhooks",
                table: "FIN_PaymentWebhooks",
                columns: new[] { "GatewayID", "EventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_CreatedBy",
                table: "FIN_Promotions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_PromotionTypeID",
                table: "FIN_Promotions",
                column: "PromotionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_UpdatedBy",
                table: "FIN_Promotions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_FIN_Promotions_Code",
                table: "FIN_Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_ContractStatusID",
                table: "HR_Contracts",
                column: "ContractStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_CreatedBy",
                table: "HR_Contracts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_TeacherID",
                table: "HR_Contracts",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_UpdatedBy",
                table: "HR_Contracts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_HR_Contracts_Code",
                table: "HR_Contracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_TeacherAvailabilities_TeacherID",
                table: "HR_TeacherAvailabilities",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_AccountRoles_RoleID",
                table: "IDN_AccountRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "UQ_IDN_AccountRoles",
                table: "IDN_AccountRoles",
                columns: new[] { "AccountID", "RoleID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Accounts_AccountStatusID",
                table: "IDN_Accounts",
                column: "AccountStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Accounts_UpdatedBy",
                table: "IDN_Accounts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_IDN_Roles_RoleName",
                table: "IDN_Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Students_UpdatedBy",
                table: "IDN_Students",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_IDN_Student_Code",
                table: "IDN_Students",
                column: "StudentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IDN_TeacherCredentials_CredentialTypeID",
                table: "IDN_TeacherCredentials",
                column: "CredentialTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_TeacherCredentials_TeacherID",
                table: "IDN_TeacherCredentials",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_TeacherCredentials_UpdatedBy",
                table: "IDN_TeacherCredentials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Teachers_UpdatedBy",
                table: "IDN_Teachers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_IDN_Teachers_Code",
                table: "IDN_Teachers",
                column: "TeacherCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_ReportStatusID",
                table: "RPT_Reports",
                column: "ReportStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_ReportTypeID",
                table: "RPT_Reports",
                column: "ReportTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_ResolvedBy",
                table: "RPT_Reports",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_SubmittedBy",
                table: "RPT_Reports",
                column: "SubmittedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_AcademicRequestHistories");

            migrationBuilder.DropTable(
                name: "ACAD_Attendances");

            migrationBuilder.DropTable(
                name: "ACAD_ClassReservations");

            migrationBuilder.DropTable(
                name: "ACAD_CoursePackageItems");

            migrationBuilder.DropTable(
                name: "ACAD_Enrollments");

            migrationBuilder.DropTable(
                name: "ACAD_LearningMaterials");

            migrationBuilder.DropTable(
                name: "ACAD_Submissions");

            migrationBuilder.DropTable(
                name: "COM_Conversations");

            migrationBuilder.DropTable(
                name: "COM_Feedback");

            migrationBuilder.DropTable(
                name: "COM_FeedbackRecords");

            migrationBuilder.DropTable(
                name: "COM_Notifications");

            migrationBuilder.DropTable(
                name: "EVT_EventFeedback");

            migrationBuilder.DropTable(
                name: "EVT_EventRegistrations");

            migrationBuilder.DropTable(
                name: "FIN_InvoiceItems");

            migrationBuilder.DropTable(
                name: "FIN_PaymentRefunds");

            migrationBuilder.DropTable(
                name: "FIN_PaymentWebhooks");

            migrationBuilder.DropTable(
                name: "HR_Contracts");

            migrationBuilder.DropTable(
                name: "HR_TeacherAvailabilities");

            migrationBuilder.DropTable(
                name: "IDN_AccountRoles");

            migrationBuilder.DropTable(
                name: "IDN_TeacherCredentials");

            migrationBuilder.DropTable(
                name: "RPT_Reports");

            migrationBuilder.DropTable(
                name: "ACAD_AcademicRequests");

            migrationBuilder.DropTable(
                name: "ACAD_Assignments");

            migrationBuilder.DropTable(
                name: "EVT_Events");

            migrationBuilder.DropTable(
                name: "ACAD_CoursePackages");

            migrationBuilder.DropTable(
                name: "FIN_Promotions");

            migrationBuilder.DropTable(
                name: "FIN_Payments");

            migrationBuilder.DropTable(
                name: "IDN_Roles");

            migrationBuilder.DropTable(
                name: "ACAD_ClassMeetings");

            migrationBuilder.DropTable(
                name: "FIN_Invoices");

            migrationBuilder.DropTable(
                name: "ACAD_Classes");

            migrationBuilder.DropTable(
                name: "ACAD_SyllabusItems");

            migrationBuilder.DropTable(
                name: "FAC_Rooms");

            migrationBuilder.DropTable(
                name: "IDN_Students");

            migrationBuilder.DropTable(
                name: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropTable(
                name: "ACAD_Syllabi");

            migrationBuilder.DropTable(
                name: "IDN_Teachers");

            migrationBuilder.DropTable(
                name: "ACAD_Courses");

            migrationBuilder.DropTable(
                name: "ACAD_CourseCategories");

            migrationBuilder.DropTable(
                name: "IDN_Accounts");

            migrationBuilder.DropTable(
                name: "CORE_LookUps");

            migrationBuilder.DropTable(
                name: "CORE_LookUpTypes");
        }
    }
}
