using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RenameNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReqHist_ChangedBy",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_Processed",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Assignments_Created",
                table: "ACAD_Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Assignments_Updated",
                table: "ACAD_Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_Updated",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Updated",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Updated",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Courses_Created",
                table: "ACAD_Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Courses_Updated",
                table: "ACAD_Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Updated",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Updated",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Updated",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Updated",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Updated",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_SyllabusItems_Created",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_SyllabusItems_Updated",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FAC_Rooms_Created",
                table: "FAC_Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_FAC_Rooms_Updated",
                table: "FAC_Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Updated",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Updated",
                table: "HR_Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_IDN_Student_Update",
                table: "IDN_Students");

            migrationBuilder.DropForeignKey(
                name: "FK_IDN_Teacher_Update",
                table: "IDN_Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCredentials_Update",
                table: "IDN_TeacherCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_RPT_Reports_Resolved",
                table: "RPT_Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_RPT_Reports_Submitter",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_RPT_Reports_ResolvedByNavigationAccountID",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_RPT_Reports_SubmittedByNavigationAccountID",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_IDN_TeacherCredentials_UpdatedByNavigationAccountID",
                table: "IDN_TeacherCredentials");

            migrationBuilder.DropIndex(
                name: "IX_IDN_Teacher_UpdatedByNavigationAccountID",
                table: "IDN_Teacher");

            migrationBuilder.DropIndex(
                name: "IX_IDN_Students_UpdatedByNavigationAccountID",
                table: "IDN_Students");

            migrationBuilder.DropIndex(
                name: "IX_HR_Contracts_CreatedByNavigationAccountID",
                table: "HR_Contracts");

            migrationBuilder.DropIndex(
                name: "IX_HR_Contracts_UpdatedByNavigationAccountID",
                table: "HR_Contracts");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Promotions_CreatedByNavigationAccountID",
                table: "FIN_Promotions");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Promotions_UpdatedByNavigationAccountID",
                table: "FIN_Promotions");

            migrationBuilder.DropIndex(
                name: "IX_FAC_Rooms_CreatedByNavigationAccountID",
                table: "FAC_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_FAC_Rooms_UpdatedByNavigationAccountID",
                table: "FAC_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_SyllabusItems_CreatedByNavigationAccountID",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_SyllabusItems_UpdatedByNavigationAccountID",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Syllabi_CreatedByNavigationAccountID",
                table: "ACAD_Syllabi");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Syllabi_UpdatedByNavigationAccountID",
                table: "ACAD_Syllabi");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Submissions_CreatedByNavigationAccountID",
                table: "ACAD_Submissions");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Submissions_UpdatedByNavigationAccountID",
                table: "ACAD_Submissions");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_LearningMaterials_CreatedByNavigationAccountID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_LearningMaterials_UpdatedByNavigationAccountID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Enrollments_CreatedByNavigationAccountID",
                table: "ACAD_Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Enrollments_UpdatedByNavigationAccountID",
                table: "ACAD_Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CreatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_CourseTeacherAssignments_UpdatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Courses_CreatedByNavigationAccountID",
                table: "ACAD_Courses");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Courses_UpdatedByNavigationAccountID",
                table: "ACAD_Courses");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_CreatedByNavigationAccountID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_UpdatedByNavigationAccountID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Classes_CreatedByNavigationAccountID",
                table: "ACAD_Classes");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Classes_UpdatedByNavigationAccountID",
                table: "ACAD_Classes");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Attendances_CheckByNavigationAccountID",
                table: "ACAD_Attendances");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Attendances_UpdatedByNavigationAccountID",
                table: "ACAD_Attendances");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Assignments_CreatedByNavigationAccountID",
                table: "ACAD_Assignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Assignments_UpdatedByNavigationAccountID",
                table: "ACAD_Assignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_ProcessedByNavigationAccountID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedByNavigationAccountID",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.DropColumn(
                name: "ResolvedByNavigationAccountID",
                table: "RPT_Reports");

            migrationBuilder.DropColumn(
                name: "SubmittedByNavigationAccountID",
                table: "RPT_Reports");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_TeacherCredentials");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_Teacher");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_Students");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "HR_Contracts");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "HR_Contracts");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "FIN_Promotions");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "FIN_Promotions");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "FAC_Rooms");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "FAC_Rooms");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Syllabi");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Syllabi");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Submissions");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Submissions");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Enrollments");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Enrollments");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Courses");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Courses");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Classes");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Classes");

            migrationBuilder.DropColumn(
                name: "CheckByNavigationAccountID",
                table: "ACAD_Attendances");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Attendances");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Assignments");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Assignments");

            migrationBuilder.DropColumn(
                name: "ProcessedByNavigationAccountID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ChangedByNavigationAccountID",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.RenameColumn(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_Accounts",
                newName: "UpdatedByNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_IDN_Accounts_UpdatedByNavigationAccountID",
                table: "IDN_Accounts",
                newName: "IX_IDN_Accounts_UpdatedByNavigationId");

            migrationBuilder.RenameColumn(
                name: "CreatedByNavigationAccountID",
                table: "COM_FeedbackRecords",
                newName: "CreatedByNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_COM_FeedbackRecords_CreatedByNavigationAccountID",
                table: "COM_FeedbackRecords",
                newName: "IX_COM_FeedbackRecords_CreatedByNavigationId");

            migrationBuilder.RenameColumn(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_CoursePackages",
                newName: "UpdatedByNavigationId");

            migrationBuilder.RenameColumn(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_CoursePackages",
                newName: "CreatedByNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CoursePackages_UpdatedByNavigationAccountID",
                table: "ACAD_CoursePackages",
                newName: "IX_ACAD_CoursePackages_UpdatedByNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CoursePackages_CreatedByNavigationAccountID",
                table: "ACAD_CoursePackages",
                newName: "IX_ACAD_CoursePackages_CreatedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_ResolvedBy",
                table: "RPT_Reports",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_SubmittedBy",
                table: "RPT_Reports",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_TeacherCredentials_UpdatedBy",
                table: "IDN_TeacherCredentials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Teacher_UpdatedBy",
                table: "IDN_Teacher",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Students_UpdatedBy",
                table: "IDN_Students",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_CreatedBy",
                table: "HR_Contracts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_UpdatedBy",
                table: "HR_Contracts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_CreatedBy",
                table: "FIN_Promotions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_UpdatedBy",
                table: "FIN_Promotions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_CreatedBy",
                table: "FIN_Invoices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_UpdatedBy",
                table: "FIN_Invoices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_CreatedBy",
                table: "FAC_Rooms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_UpdatedBy",
                table: "FAC_Rooms",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_CreatedBy",
                table: "ACAD_SyllabusItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_UpdatedBy",
                table: "ACAD_SyllabusItems",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_CreatedBy",
                table: "ACAD_Syllabi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_UpdatedBy",
                table: "ACAD_Syllabi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_CreatedBy",
                table: "ACAD_Submissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_UpdatedBy",
                table: "ACAD_Submissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_CreatedBy",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_UpdatedBy",
                table: "ACAD_LearningMaterials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_CreatedBy",
                table: "ACAD_Enrollments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_UpdatedBy",
                table: "ACAD_Enrollments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CreatedBy",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_UpdatedBy",
                table: "ACAD_CourseTeacherAssignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CreatedBy",
                table: "ACAD_Courses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_UpdatedBy",
                table: "ACAD_Courses",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_CreatedBy",
                table: "ACAD_ClassMeetings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_UpdatedBy",
                table: "ACAD_ClassMeetings",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_CreatedBy",
                table: "ACAD_Classes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_UpdatedBy",
                table: "ACAD_Classes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_CheckBy",
                table: "ACAD_Attendances",
                column: "CheckBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_UpdatedBy",
                table: "ACAD_Attendances",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_CreatedBy",
                table: "ACAD_Assignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_UpdatedBy",
                table: "ACAD_Assignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ProcessedBy",
                table: "ACAD_AcademicRequests",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                column: "ChangedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReqHist_ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                column: "ChangedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_Processed",
                table: "ACAD_AcademicRequests",
                column: "ProcessedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Assignments_Created",
                table: "ACAD_Assignments",
                column: "CreatedBy",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Assignments_Updated",
                table: "ACAD_Assignments",
                column: "UpdatedBy",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances",
                column: "CheckBy",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_Updated",
                table: "ACAD_Attendances",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Updated",
                table: "ACAD_Classes",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Updated",
                table: "ACAD_ClassMeetings",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Courses_Created",
                table: "ACAD_Courses",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Courses_Updated",
                table: "ACAD_Courses",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Updated",
                table: "ACAD_CourseTeacherAssignments",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Updated",
                table: "ACAD_Enrollments",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Updated",
                table: "ACAD_LearningMaterials",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Updated",
                table: "ACAD_Submissions",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Updated",
                table: "ACAD_Syllabi",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_SyllabusItems_Created",
                table: "ACAD_SyllabusItems",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_SyllabusItems_Updated",
                table: "ACAD_SyllabusItems",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAC_Rooms_Created",
                table: "FAC_Rooms",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAC_Rooms_Updated",
                table: "FAC_Rooms",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_UpdatedBy",
                table: "FIN_Invoices",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Updated",
                table: "FIN_Promotions",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Updated",
                table: "HR_Contracts",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDN_Student_Update",
                table: "IDN_Students",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDN_Teacher_Update",
                table: "IDN_Teacher",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCredentials_Update",
                table: "IDN_TeacherCredentials",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_RPT_Reports_Resolved",
                table: "RPT_Reports",
                column: "ResolvedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_RPT_Reports_Submitter",
                table: "RPT_Reports",
                column: "SubmittedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReqHist_ChangedBy",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_Processed",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Assignments_Created",
                table: "ACAD_Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Assignments_Updated",
                table: "ACAD_Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Attendance_Updated",
                table: "ACAD_Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_Updated",
                table: "ACAD_Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Updated",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Courses_Created",
                table: "ACAD_Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Courses_Updated",
                table: "ACAD_Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Updated",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Updated",
                table: "ACAD_Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Updated",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Submissions_Updated",
                table: "ACAD_Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Syllabi_Updated",
                table: "ACAD_Syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_SyllabusItems_Created",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_SyllabusItems_Updated",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FAC_Rooms_Created",
                table: "FAC_Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_FAC_Rooms_Updated",
                table: "FAC_Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_CreatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Invoices_IDN_Accounts_UpdatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Promotions_Updated",
                table: "FIN_Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Contracts_Updated",
                table: "HR_Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_IDN_Student_Update",
                table: "IDN_Students");

            migrationBuilder.DropForeignKey(
                name: "FK_IDN_Teacher_Update",
                table: "IDN_Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCredentials_Update",
                table: "IDN_TeacherCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_RPT_Reports_Resolved",
                table: "RPT_Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_RPT_Reports_Submitter",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_RPT_Reports_ResolvedBy",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_RPT_Reports_SubmittedBy",
                table: "RPT_Reports");

            migrationBuilder.DropIndex(
                name: "IX_IDN_TeacherCredentials_UpdatedBy",
                table: "IDN_TeacherCredentials");

            migrationBuilder.DropIndex(
                name: "IX_IDN_Teacher_UpdatedBy",
                table: "IDN_Teacher");

            migrationBuilder.DropIndex(
                name: "IX_IDN_Students_UpdatedBy",
                table: "IDN_Students");

            migrationBuilder.DropIndex(
                name: "IX_HR_Contracts_CreatedBy",
                table: "HR_Contracts");

            migrationBuilder.DropIndex(
                name: "IX_HR_Contracts_UpdatedBy",
                table: "HR_Contracts");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Promotions_CreatedBy",
                table: "FIN_Promotions");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Promotions_UpdatedBy",
                table: "FIN_Promotions");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_CreatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_UpdatedBy",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_FAC_Rooms_CreatedBy",
                table: "FAC_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_FAC_Rooms_UpdatedBy",
                table: "FAC_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_SyllabusItems_CreatedBy",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_SyllabusItems_UpdatedBy",
                table: "ACAD_SyllabusItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Syllabi_CreatedBy",
                table: "ACAD_Syllabi");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Syllabi_UpdatedBy",
                table: "ACAD_Syllabi");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Submissions_CreatedBy",
                table: "ACAD_Submissions");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Submissions_UpdatedBy",
                table: "ACAD_Submissions");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_LearningMaterials_CreatedBy",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_LearningMaterials_UpdatedBy",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Enrollments_CreatedBy",
                table: "ACAD_Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Enrollments_UpdatedBy",
                table: "ACAD_Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CreatedBy",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_CourseTeacherAssignments_UpdatedBy",
                table: "ACAD_CourseTeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Courses_CreatedBy",
                table: "ACAD_Courses");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Courses_UpdatedBy",
                table: "ACAD_Courses");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_CreatedBy",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_UpdatedBy",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Classes_CreatedBy",
                table: "ACAD_Classes");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Classes_UpdatedBy",
                table: "ACAD_Classes");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Attendances_CheckBy",
                table: "ACAD_Attendances");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Attendances_UpdatedBy",
                table: "ACAD_Attendances");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Assignments_CreatedBy",
                table: "ACAD_Assignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Assignments_UpdatedBy",
                table: "ACAD_Assignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_ProcessedBy",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedBy",
                table: "ACAD_AcademicRequestHistories");

            migrationBuilder.RenameColumn(
                name: "UpdatedByNavigationId",
                table: "IDN_Accounts",
                newName: "UpdatedByNavigationAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_IDN_Accounts_UpdatedByNavigationId",
                table: "IDN_Accounts",
                newName: "IX_IDN_Accounts_UpdatedByNavigationAccountID");

            migrationBuilder.RenameColumn(
                name: "CreatedByNavigationId",
                table: "COM_FeedbackRecords",
                newName: "CreatedByNavigationAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_COM_FeedbackRecords_CreatedByNavigationId",
                table: "COM_FeedbackRecords",
                newName: "IX_COM_FeedbackRecords_CreatedByNavigationAccountID");

            migrationBuilder.RenameColumn(
                name: "UpdatedByNavigationId",
                table: "ACAD_CoursePackages",
                newName: "UpdatedByNavigationAccountID");

            migrationBuilder.RenameColumn(
                name: "CreatedByNavigationId",
                table: "ACAD_CoursePackages",
                newName: "CreatedByNavigationAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CoursePackages_UpdatedByNavigationId",
                table: "ACAD_CoursePackages",
                newName: "IX_ACAD_CoursePackages_UpdatedByNavigationAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CoursePackages_CreatedByNavigationId",
                table: "ACAD_CoursePackages",
                newName: "IX_ACAD_CoursePackages_CreatedByNavigationAccountID");

            migrationBuilder.AddColumn<Guid>(
                name: "ResolvedByNavigationAccountID",
                table: "RPT_Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubmittedByNavigationAccountID",
                table: "RPT_Reports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_TeacherCredentials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_Teacher",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "IDN_Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "HR_Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "HR_Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "FIN_Promotions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "FIN_Promotions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "FAC_Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "FAC_Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_SyllabusItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_SyllabusItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Syllabi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Syllabi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Submissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Submissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_LearningMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_LearningMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckByNavigationAccountID",
                table: "ACAD_Attendances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Attendances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationAccountID",
                table: "ACAD_Assignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationAccountID",
                table: "ACAD_Assignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessedByNavigationAccountID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedByNavigationAccountID",
                table: "ACAD_AcademicRequestHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_ResolvedByNavigationAccountID",
                table: "RPT_Reports",
                column: "ResolvedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_RPT_Reports_SubmittedByNavigationAccountID",
                table: "RPT_Reports",
                column: "SubmittedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_TeacherCredentials_UpdatedByNavigationAccountID",
                table: "IDN_TeacherCredentials",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Teacher_UpdatedByNavigationAccountID",
                table: "IDN_Teacher",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_IDN_Students_UpdatedByNavigationAccountID",
                table: "IDN_Students",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_CreatedByNavigationAccountID",
                table: "HR_Contracts",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Contracts_UpdatedByNavigationAccountID",
                table: "HR_Contracts",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_CreatedByNavigationAccountID",
                table: "FIN_Promotions",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Promotions_UpdatedByNavigationAccountID",
                table: "FIN_Promotions",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_CreatedByNavigationAccountID",
                table: "FAC_Rooms",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_UpdatedByNavigationAccountID",
                table: "FAC_Rooms",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_CreatedByNavigationAccountID",
                table: "ACAD_SyllabusItems",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_SyllabusItems_UpdatedByNavigationAccountID",
                table: "ACAD_SyllabusItems",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_CreatedByNavigationAccountID",
                table: "ACAD_Syllabi",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Syllabi_UpdatedByNavigationAccountID",
                table: "ACAD_Syllabi",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_CreatedByNavigationAccountID",
                table: "ACAD_Submissions",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Submissions_UpdatedByNavigationAccountID",
                table: "ACAD_Submissions",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_CreatedByNavigationAccountID",
                table: "ACAD_LearningMaterials",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_UpdatedByNavigationAccountID",
                table: "ACAD_LearningMaterials",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_CreatedByNavigationAccountID",
                table: "ACAD_Enrollments",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_UpdatedByNavigationAccountID",
                table: "ACAD_Enrollments",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_CreatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseTeacherAssignments_UpdatedByNavigationAccountID",
                table: "ACAD_CourseTeacherAssignments",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_CreatedByNavigationAccountID",
                table: "ACAD_Courses",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Courses_UpdatedByNavigationAccountID",
                table: "ACAD_Courses",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_CreatedByNavigationAccountID",
                table: "ACAD_ClassMeetings",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_UpdatedByNavigationAccountID",
                table: "ACAD_ClassMeetings",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_CreatedByNavigationAccountID",
                table: "ACAD_Classes",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_UpdatedByNavigationAccountID",
                table: "ACAD_Classes",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_CheckByNavigationAccountID",
                table: "ACAD_Attendances",
                column: "CheckByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Attendances_UpdatedByNavigationAccountID",
                table: "ACAD_Attendances",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_CreatedByNavigationAccountID",
                table: "ACAD_Assignments",
                column: "CreatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_UpdatedByNavigationAccountID",
                table: "ACAD_Assignments",
                column: "UpdatedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ProcessedByNavigationAccountID",
                table: "ACAD_AcademicRequests",
                column: "ProcessedByNavigationAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequestHistories_ChangedByNavigationAccountID",
                table: "ACAD_AcademicRequestHistories",
                column: "ChangedByNavigationAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReqHist_ChangedBy",
                table: "ACAD_AcademicRequestHistories",
                column: "ChangedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_Processed",
                table: "ACAD_AcademicRequests",
                column: "ProcessedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Assignments_Created",
                table: "ACAD_Assignments",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Assignments_Updated",
                table: "ACAD_Assignments",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_Created",
                table: "ACAD_Attendances",
                column: "CheckByNavigationAccountID",
                principalTable: "IDN_Teacher",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Attendance_Updated",
                table: "ACAD_Attendances",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Created",
                table: "ACAD_Classes",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_Updated",
                table: "ACAD_Classes",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Created",
                table: "ACAD_ClassMeetings",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Updated",
                table: "ACAD_ClassMeetings",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Courses_Created",
                table: "ACAD_Courses",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Courses_Updated",
                table: "ACAD_Courses",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Created",
                table: "ACAD_CourseTeacherAssignments",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_CourseTeacherAssignments_Updated",
                table: "ACAD_CourseTeacherAssignments",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Created",
                table: "ACAD_Enrollments",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Updated",
                table: "ACAD_Enrollments",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Updated",
                table: "ACAD_LearningMaterials",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Created",
                table: "ACAD_Submissions",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Submissions_Updated",
                table: "ACAD_Submissions",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Created",
                table: "ACAD_Syllabi",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Syllabi_Updated",
                table: "ACAD_Syllabi",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_SyllabusItems_Created",
                table: "ACAD_SyllabusItems",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_SyllabusItems_Updated",
                table: "ACAD_SyllabusItems",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAC_Rooms_Created",
                table: "FAC_Rooms",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FAC_Rooms_Updated",
                table: "FAC_Rooms",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Created",
                table: "FIN_Promotions",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Promotions_Updated",
                table: "FIN_Promotions",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Created",
                table: "HR_Contracts",
                column: "CreatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Contracts_Updated",
                table: "HR_Contracts",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDN_Student_Update",
                table: "IDN_Students",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDN_Teacher_Update",
                table: "IDN_Teacher",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCredentials_Update",
                table: "IDN_TeacherCredentials",
                column: "UpdatedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_RPT_Reports_Resolved",
                table: "RPT_Reports",
                column: "ResolvedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_RPT_Reports_Submitter",
                table: "RPT_Reports",
                column: "SubmittedByNavigationAccountID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }
    }
}
