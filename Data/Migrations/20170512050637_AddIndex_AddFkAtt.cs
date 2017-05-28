using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreUniversity.Data.Migrations
{
    public partial class AddIndex_AddFkAtt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Classes_ClassID",
                table: "ClassStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Students_StudentID",
                table: "ClassStudent");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teachers_Surname",
                table: "Teachers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Students_Surname",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "PersonID",
                table: "Teachers",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "PersonID",
                table: "Students",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "ClassStudent",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "ClassStudent",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudent_StudentID",
                table: "ClassStudent",
                newName: "IX_ClassStudent_StudentId");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "Classes",
                newName: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Surname",
                table: "Teachers",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Surname",
                table: "Students",
                column: "Surname");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Classes_ClassId",
                table: "ClassStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Students_StudentId",
                table: "ClassStudent");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_Surname",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_Surname",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Teachers",
                newName: "PersonID");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Students",
                newName: "PersonID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "ClassStudent",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "ClassStudent",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudent_StudentId",
                table: "ClassStudent",
                newName: "IX_ClassStudent_StudentID");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Classes",
                newName: "ClassID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teachers_Surname",
                table: "Teachers",
                column: "Surname");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Students_Surname",
                table: "Students",
                column: "Surname");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Classes_ClassID",
                table: "ClassStudent",
                column: "ClassID",
                principalTable: "Classes",
                principalColumn: "ClassID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Students_StudentID",
                table: "ClassStudent",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "PersonID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
