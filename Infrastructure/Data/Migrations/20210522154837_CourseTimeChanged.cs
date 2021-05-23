using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class CourseTimeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseTimes_Courses_CourseId",
                table: "CourseTimes");

            migrationBuilder.DropColumn(
                name: "AcademicYearFrom",
                table: "CourseTimes");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CourseTimes",
                newName: "TimeTo");

            migrationBuilder.RenameColumn(
                name: "AcademicYearTo",
                table: "CourseTimes",
                newName: "StudentCourseId");

            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                table: "StudentCourses",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseTimes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Classroom",
                table: "CourseTimes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeFrom",
                table: "CourseTimes",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_CourseTimes_StudentCourseId",
                table: "CourseTimes",
                column: "StudentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTimes_Courses_CourseId",
                table: "CourseTimes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTimes_StudentCourses_StudentCourseId",
                table: "CourseTimes",
                column: "StudentCourseId",
                principalTable: "StudentCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseTimes_Courses_CourseId",
                table: "CourseTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTimes_StudentCourses_StudentCourseId",
                table: "CourseTimes");

            migrationBuilder.DropIndex(
                name: "IX_CourseTimes_StudentCourseId",
                table: "CourseTimes");

            migrationBuilder.DropColumn(
                name: "Teacher",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "Classroom",
                table: "CourseTimes");

            migrationBuilder.DropColumn(
                name: "TimeFrom",
                table: "CourseTimes");

            migrationBuilder.RenameColumn(
                name: "TimeTo",
                table: "CourseTimes",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "StudentCourseId",
                table: "CourseTimes",
                newName: "AcademicYearTo");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearFrom",
                table: "CourseTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTimes_Courses_CourseId",
                table: "CourseTimes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
