﻿// <auto-generated />
using System;
using BookingBirthday.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingBirthday.Data.Migrations
{
    [DbContext(typeof(BookingDbContext))]
    [Migration("20240329063834_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingBirthday.Data.Entities.Age", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Age", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Area", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookingStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Processing");

                    b.Property<DateTime>("ChildDateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChildName")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChildNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date_cancel")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_order")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_start")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Category_requests", b =>
                {
                    b.Property<int>("category_request_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("category_request_id"));

                    b.Property<string>("category_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("guest_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("host_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("is_approved")
                        .HasColumnType("int");

                    b.Property<bool>("is_deleted_by_admin")
                        .HasColumnType("bit");

                    b.Property<bool>("is_deleted_by_owner")
                        .HasColumnType("bit");

                    b.Property<bool>("is_viewed_by_admin")
                        .HasColumnType("bit");

                    b.Property<bool>("is_viewed_by_owner")
                        .HasColumnType("bit");

                    b.Property<string>("mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rejection_reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("report")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("requester_id")
                        .HasColumnType("int");

                    b.HasKey("category_request_id");

                    b.ToTable("Category_Requests");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AreaId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Location", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgeId")
                        .HasColumnType("int");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Host_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PackageTypeId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image_url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AgeId");

                    b.HasIndex("PackageTypeId");

                    b.HasIndex("ThemeId");

                    b.HasIndex("UserId");

                    b.ToTable("Package", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("PackageId");

                    b.ToTable("PackageLocation", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PackageType", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VnPayResponseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Rate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Theme", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ResetPasswordCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Address");

                    b.HasIndex("Name");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasFilter("[Phone] IS NOT NULL");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Location", "Location")
                        .WithMany("Bookings")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Package", "Package")
                        .WithMany("Bookings")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Location", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Area", "Area")
                        .WithMany("Location")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Area");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Age", "Age")
                        .WithMany("Package")
                        .HasForeignKey("AgeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.PackageType", "PackageType")
                        .WithMany("Package")
                        .HasForeignKey("PackageTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Theme", "Theme")
                        .WithMany("Package")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.User", "User")
                        .WithMany("Packages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Age");

                    b.Navigation("PackageType");

                    b.Navigation("Theme");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageLocation", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Location", "Location")
                        .WithMany("PackageLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Package", "Package")
                        .WithMany("PackageLocations")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Payment", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Rate", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Package", "Packages")
                        .WithMany("Rates")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.User", "Users")
                        .WithMany("Rates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Packages");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Theme", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.User", "User")
                        .WithMany("Theme")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Age", b =>
                {
                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Area", b =>
                {
                    b.Navigation("Location");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Location", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("PackageLocations");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("PackageLocations");

                    b.Navigation("Rates");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageType", b =>
                {
                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Theme", b =>
                {
                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Packages");

                    b.Navigation("Rates");

                    b.Navigation("Theme");
                });
#pragma warning restore 612, 618
        }
    }
}
