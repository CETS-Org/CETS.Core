using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuditingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CoursePackages_Created",
                table: "ACAD_CoursePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_COM_FeedbackRecord_Created",
                table: "COM_FeedbackRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "EVT_EventFeedback",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "COM_FeedbackRecords",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "COM_Feedback",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "StartAt",
                table: "COM_Conversations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "ACAD_Submissions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "ACAD_LearningMaterials",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CheckBy",
                table: "ACAD_Attendances",
                newName: "CheckedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_Attendances_CheckBy",
                table: "ACAD_Attendances",
                newName: "IX_ACAD_Attendances_CheckedBy");

            migrationBuilder.RenameColumn(
                name: "ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                newName: "UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                newName: "IX_ACAD_AcademicRequestHistories_UpdatedBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "HR_Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Promotions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_PaymentRefunds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FAC_Rooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EVT_Events",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "EVT_Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EVT_Events",
                type: "datetime2(0)",
                precision: 0,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "EVT_Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "CORE_LookUps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "COM_FeedbackRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_SyllabusItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Syllabi",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Submissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_LearningMaterials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Enrollments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_CourseTeacherAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedAt",
                table: "ACAD_CourseTeacherAssignments",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0,
                oldDefaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ACAD_CourseTeacherAssignments",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_CoursePackages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Classes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ACAD_AcademicRequestHistories",
                type: "datetime2(0)",
                precision: 0,
                nullable: true,
                defaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_CheckedBy",
                table: "ACAD_Attendances",
                column: "CheckedBy",
                principalTable: "IDN_Teachers",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CoursePackages_Created",
                table: "ACAD_CoursePackages",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_COM_FeedbackRecord_Created",
                table: "COM_FeedbackRecords",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_CheckedBy",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CoursePackages_Created",
                table: "ACAD_CoursePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_COM_FeedbackRecord_Created",
                table: "COM_FeedbackRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EVT_Events");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EVT_Events");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EVT_Events");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EVT_Events");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EVT_EventFeedback",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "COM_FeedbackRecords",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "COM_Feedback",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "COM_Conversations",
                newName: "StartAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ACAD_Submissions",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ACAD_LearningMaterials",
                newName: "UploadDate");

            migrationBuilder.RenameColumn(
                name: "CheckedBy",
                table: "ACAD_Attendances",
                newName: "CheckBy");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_Attendances_CheckedBy",
                table: "ACAD_Attendances",
                newName: "IX_ACAD_Attendances_CheckBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ACAD_AcademicRequestHistories",
                newName: "ChangedBy");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_AcademicRequestHistories_UpdatedBy",
                table: "ACAD_AcademicRequestHistories",
                newName: "IX_ACAD_AcademicRequestHistories_ChangedBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "HR_Contracts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Promotions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Payments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_PaymentRefunds",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FIN_Invoices",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "FAC_Rooms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "CORE_LookUps",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "COM_FeedbackRecords",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_SyllabusItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Syllabi",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Submissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_LearningMaterials",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Enrollments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_CourseTeacherAssignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedAt",
                table: "ACAD_CourseTeacherAssignments",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(sysutcdatetime())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Courses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_CoursePackages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACAD_Classes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "ACAD_AcademicRequestHistories",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances",
                column: "CheckBy",
                principalTable: "IDN_Teachers",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CoursePackages_Created",
                table: "ACAD_CoursePackages",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_COM_FeedbackRecord_Created",
                table: "COM_FeedbackRecords",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }
    }
}
