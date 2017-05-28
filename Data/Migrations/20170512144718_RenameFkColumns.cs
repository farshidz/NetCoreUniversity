using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreUniversity.Data.Migrations
{
    public partial class RenameFkColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Students_StudentId",
                table: "ClassStudents");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "ClassStudents",
                newName: "StudentPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudents_StudentId",
                table: "ClassStudents",
                newName: "IX_ClassStudents_StudentPersonId");

            migrationBuilder.RenameColumn(
                name: "TeacherPersonID",
                table: "Classes",
                newName: "TeacherPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_TeacherPersonID",
                table: "Classes",
                newName: "IX_Classes_TeacherPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonId",
                table: "Classes",
                column: "TeacherPersonId",
                principalTable: "Teachers",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Students_StudentPersonId",
                table: "ClassStudents",
                column: "StudentPersonId",
                principalTable: "Students",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Students_StudentPersonId",
                table: "ClassStudents");

            migrationBuilder.RenameColumn(
                name: "StudentPersonId",
                table: "ClassStudents",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudents_StudentPersonId",
                table: "ClassStudents",
                newName: "IX_ClassStudents_StudentId");

            migrationBuilder.RenameColumn(
                name: "TeacherPersonId",
                table: "Classes",
                newName: "TeacherPersonID");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_TeacherPersonId",
                table: "Classes",
                newName: "IX_Classes_TeacherPersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonID",
                table: "Classes",
                column: "TeacherPersonID",
                principalTable: "Teachers",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Students_StudentId",
                table: "ClassStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
