using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreUniversity.Data.Migrations
{
    public partial class RenameClassStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Classes_ClassId",
                table: "ClassStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Students_StudentId",
                table: "ClassStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassStudent",
                table: "ClassStudent");

            migrationBuilder.RenameTable(
                name: "ClassStudent",
                newName: "ClassStudents");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudent_StudentId",
                table: "ClassStudents",
                newName: "IX_ClassStudents_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassStudents",
                table: "ClassStudents",
                columns: new[] { "ClassId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Classes_ClassId",
                table: "ClassStudents",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Students_StudentId",
                table: "ClassStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Classes_ClassId",
                table: "ClassStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Students_StudentId",
                table: "ClassStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassStudents",
                table: "ClassStudents");

            migrationBuilder.RenameTable(
                name: "ClassStudents",
                newName: "ClassStudent");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudents_StudentId",
                table: "ClassStudent",
                newName: "IX_ClassStudent_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassStudent",
                table: "ClassStudent",
                columns: new[] { "ClassId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Classes_ClassId",
                table: "ClassStudent",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Students_StudentId",
                table: "ClassStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
