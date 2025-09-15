using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAverageRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "ACAD_Courses",
                type: "decimal(3,2)",
                nullable: true);


            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_COM_Feedback_UpdateCourseRating
                ON dbo.COM_Feedback
                AFTER INSERT, UPDATE, DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @CourseFeedbackTypeID UNIQUEIDENTIFIER;
                    SELECT @CourseFeedbackTypeID = lt.LookUpID
                    FROM dbo.CORE_LookUps lt
                    INNER JOIN dbo.CORE_LookUpTypes ltt ON lt.LookUpTypeID = ltt.LookUpTypeID
                    WHERE ltt.Code = 'FeedbackType' AND lt.Name = 'Course';

                    DECLARE @AffectedCourseIDs TABLE (CourseID UNIQUEIDENTIFIER PRIMARY KEY);

                    INSERT INTO @AffectedCourseIDs (CourseID)
                    SELECT DISTINCT CourseID 
                    FROM inserted
                    WHERE CourseID IS NOT NULL;

                    INSERT INTO @AffectedCourseIDs (CourseID)
                    SELECT DISTINCT CourseID 
                    FROM deleted
                    WHERE CourseID IS NOT NULL
                      AND CourseID NOT IN (SELECT CourseID FROM @AffectedCourseIDs);

                    UPDATE C
                    SET C.AverageRating = (
                        SELECT AVG(CAST(F.Rating AS DECIMAL(3, 2)))
                        FROM dbo.COM_Feedback F
                        WHERE F.CourseID = C.CourseID 
                          AND F.FeedbackTypeID = @CourseFeedbackTypeID
                          AND F.Rating IS NOT NULL 
                          AND F.IsDeleted = 0
                    )
                    FROM dbo.ACAD_Courses C
                    WHERE C.CourseID IN (SELECT CourseID FROM @AffectedCourseIDs);
                END
            ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the trigger
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS dbo.TRG_COM_Feedback_UpdateCourseRating");

            // Drop the AverageRating column
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "ACAD_Courses");

        }
    }
}
