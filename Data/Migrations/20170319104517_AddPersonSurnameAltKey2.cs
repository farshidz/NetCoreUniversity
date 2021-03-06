﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NetCoreUniversity.Data.Migrations
{
    public partial class AddPersonSurnameAltKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Person_TeacherPersonID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Person_StudentID",
                table: "ClassStudent");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dob = table.Column<DateTime>(nullable: false),
                    GivenName = table.Column<string>(maxLength: 50, nullable: false),
                    Gpa = table.Column<float>(nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.PersonID);
                    table.UniqueConstraint("AK_Students_Surname", x => x.Surname);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dob = table.Column<DateTime>(nullable: false),
                    GivenName = table.Column<string>(maxLength: 50, nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.PersonID);
                    table.UniqueConstraint("AK_Teachers_Surname", x => x.Surname);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonID",
                table: "Classes",
                column: "TeacherPersonID",
                principalTable: "Teachers",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Students_StudentID",
                table: "ClassStudent",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherPersonID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudent_Students_StudentID",
                table: "ClassStudent");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Dob = table.Column<DateTime>(nullable: false),
                    GivenName = table.Column<string>(maxLength: 50, nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false),
                    Gpa = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonID);
                    table.UniqueConstraint("AK_Person_Surname", x => x.Surname);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Person_TeacherPersonID",
                table: "Classes",
                column: "TeacherPersonID",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudent_Person_StudentID",
                table: "ClassStudent",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
