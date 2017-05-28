using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreUniversity.Data;

namespace NetCoreUniversity.Data.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20170315102226_AddAll")]
    partial class AddAll
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentsApp.Models.Class", b =>
                {
                    b.Property<int>("ClassID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int?>("TeacherPersonID");

                    b.HasKey("ClassID");

                    b.HasIndex("TeacherPersonID");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("StudentsApp.Models.ClassStudent", b =>
                {
                    b.Property<int>("ClassID");

                    b.Property<int>("StudentID");

                    b.HasKey("ClassID", "StudentID");

                    b.HasIndex("StudentID");

                    b.ToTable("ClassStudent");
                });

            modelBuilder.Entity("StudentsApp.Models.Student", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Dob");

                    b.Property<string>("GivenName")
                        .HasMaxLength(50);

                    b.Property<float>("Gpa");

                    b.Property<string>("Surname")
                        .HasMaxLength(50);

                    b.HasKey("PersonId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentsApp.Models.Teacher", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Dob");

                    b.Property<string>("GivenName")
                        .HasMaxLength(50);

                    b.Property<string>("Surname")
                        .HasMaxLength(50);

                    b.HasKey("PersonId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("StudentsApp.Models.Class", b =>
                {
                    b.HasOne("StudentsApp.Models.Teacher", "Teacher")
                        .WithMany("Classes")
                        .HasForeignKey("TeacherPersonID");
                });

            modelBuilder.Entity("StudentsApp.Models.ClassStudent", b =>
                {
                    b.HasOne("StudentsApp.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StudentsApp.Models.Student", "Student")
                        .WithMany("Classes")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
