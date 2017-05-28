using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreUniversity.Data;

namespace NetCoreUniversity.Data.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20170319103554_AddPersonSurnameAltKey")]
    partial class AddPersonSurnameAltKey
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
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
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

            modelBuilder.Entity("StudentsApp.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime>("Dob");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("PersonId");

                    b.HasAlternateKey("Surname");

                    b.ToTable("Person");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Person");
                });

            modelBuilder.Entity("StudentsApp.Models.Student", b =>
                {
                    b.HasBaseType("StudentsApp.Models.Person");

                    b.Property<float>("Gpa");

                    b.ToTable("Student");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("StudentsApp.Models.Teacher", b =>
                {
                    b.HasBaseType("StudentsApp.Models.Person");


                    b.ToTable("Teacher");

                    b.HasDiscriminator().HasValue("Teacher");
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
                        .WithMany("ClassStudents")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StudentsApp.Models.Student", "Student")
                        .WithMany("ClassStudents")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
