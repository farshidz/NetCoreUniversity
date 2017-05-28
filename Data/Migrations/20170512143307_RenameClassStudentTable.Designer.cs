using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreUniversity.Data;

namespace NetCoreUniversity.Data.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20170512143307_RenameClassStudentTable")]
    partial class RenameClassStudentTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentsApp.Models.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("TeacherId")
                        .HasColumnName("TeacherPersonID");

                    b.HasKey("ClassId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("StudentsApp.Models.ClassStudent", b =>
                {
                    b.Property<int>("ClassId");

                    b.Property<int>("StudentId");

                    b.HasKey("ClassId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("ClassStudents");
                });

            modelBuilder.Entity("StudentsApp.Models.Student", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Dob");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<float>("Gpa");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("PersonId");

                    b.HasIndex("Surname");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentsApp.Models.Teacher", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Dob");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("PersonId");

                    b.HasIndex("Surname");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("StudentsApp.Models.Class", b =>
                {
                    b.HasOne("StudentsApp.Models.Teacher", "Teacher")
                        .WithMany("Classes")
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("StudentsApp.Models.ClassStudent", b =>
                {
                    b.HasOne("StudentsApp.Models.Class", "Class")
                        .WithMany("ClassStudents")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StudentsApp.Models.Student", "Student")
                        .WithMany("ClassStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
