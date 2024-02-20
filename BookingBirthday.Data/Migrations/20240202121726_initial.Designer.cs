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
    [Migration("20240202121726_initial")]
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

            modelBuilder.Entity("BookingBirthday.Data.Entities.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Bill", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookingId = 1,
                            Date = new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Discount = 0.0,
                            Total = 0.0
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BillId")
                        .HasColumnType("int");

                    b.Property<int>("BookingStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BillId")
                        .IsUnique();

                    b.HasIndex("GuestId");

                    b.HasIndex("HostId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("Booking", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BillId = 1,
                            BookingStatus = 0,
                            Date = new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestId = 1,
                            HostId = 1,
                            PaymentId = 1,
                            Total = 0.0
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.BookingPackage", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.HasKey("BookingId", "PackageId");

                    b.HasIndex("PackageId");

                    b.ToTable("BookingPackage", (string)null);

                    b.HasData(
                        new
                        {
                            BookingId = 1,
                            PackageId = 1
                        },
                        new
                        {
                            BookingId = 1,
                            PackageId = 2
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.BookingService", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("BookingId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("BookingService", (string)null);

                    b.HasData(
                        new
                        {
                            BookingId = 1,
                            ServiceId = 1
                        },
                        new
                        {
                            BookingId = 1,
                            ServiceId = 2
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("GuestId")
                        .IsUnique();

                    b.ToTable("Cart", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GuestId = 1,
                            Total = 0.0
                        },
                        new
                        {
                            Id = 2,
                            GuestId = 2,
                            Total = 0.0
                        },
                        new
                        {
                            Id = 3,
                            GuestId = 3,
                            Total = 0.0
                        },
                        new
                        {
                            Id = 4,
                            GuestId = 4,
                            Total = 0.0
                        },
                        new
                        {
                            Id = 5,
                            GuestId = 5,
                            Total = 0.0
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.CartPackage", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.HasKey("CartId", "PackageId");

                    b.HasIndex("PackageId");

                    b.ToTable("CartPackage", (string)null);

                    b.HasData(
                        new
                        {
                            CartId = 1,
                            PackageId = 1
                        },
                        new
                        {
                            CartId = 2,
                            PackageId = 2
                        },
                        new
                        {
                            CartId = 3,
                            PackageId = 3
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.CartService", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("CartId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("CartService", (string)null);

                    b.HasData(
                        new
                        {
                            CartId = 1,
                            ServiceId = 1
                        },
                        new
                        {
                            CartId = 2,
                            ServiceId = 2
                        },
                        new
                        {
                            CartId = 3,
                            ServiceId = 3
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CartId")
                        .IsUnique();

                    b.ToTable("Guest", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Đường Nguyễn Văn Linh, Phường 1, Quận 10, Thành phố Hồ Chí Minh",
                            CartId = 1,
                            DateOfBirth = new DateTime(1989, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "example123@gmail.com",
                            Gender = 0,
                            Name = "Nguyễn Tấn Đạt",
                            Phone = "0961234567"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Đường Lê Lợi, Phường 2, Quận Bình Thạnh, Thành phố Hồ Chí Minh",
                            CartId = 2,
                            DateOfBirth = new DateTime(1995, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user4567@gmail.com",
                            Gender = 1,
                            Name = "Trần Thị Ngọc Mai",
                            Phone = "0902345678"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Đường Trần Hưng Đạo, Phường 3, Quận 5, Thành phố Hồ Chí Minh",
                            CartId = 3,
                            DateOfBirth = new DateTime(1983, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "testing789@gmail.com",
                            Gender = 0,
                            Name = "Lê Văn Hiếu",
                            Phone = "0933456789"
                        },
                        new
                        {
                            Id = 4,
                            Address = "234 Đường Cách Mạng Tháng Tám, Phường 4, Quận 3, Thành phố Hồ Chí Minh",
                            CartId = 4,
                            DateOfBirth = new DateTime(1990, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "randomuser12@gmail.com",
                            Gender = 1,
                            Name = "Phạm Tuyết Hoa",
                            Phone = "0914567890"
                        },
                        new
                        {
                            Id = 5,
                            Address = "567 Đường Hoàng Hoa Thám, Phường 5, Quận 6, Thành phố Hồ Chí Minh",
                            CartId = 5,
                            DateOfBirth = new DateTime(1976, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "email34@gmail.com",
                            Gender = 1,
                            Name = "Nguyễn Thị Anh",
                            Phone = "0975678901"
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Host", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Host", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "890 Đường Nguyễn Công Trứ, Phường 6, Quận 1, Thành phố Hồ Chí Minh",
                            DateOfBirth = new DateTime(1982, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "demo5678@gmail.com",
                            Gender = 0,
                            Name = "Võ Quốc Anh",
                            Phone = "0926789012"
                        },
                        new
                        {
                            Id = 2,
                            Address = "321 Đường Lý Thường Kiệt, Phường 7, Quận Tân Bình, Thành phố Hồ Chí Minh",
                            DateOfBirth = new DateTime(1998, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "username901@gmail.com",
                            Gender = 0,
                            Name = "Hoàng Văn Quân",
                            Phone = "0947890123"
                        },
                        new
                        {
                            Id = 3,
                            Address = "654 Đường Trần Phú, Phường 8, Quận Gò Vấp, Thành phố Hồ Chí Minh",
                            DateOfBirth = new DateTime(1974, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sample23@gmail.com",
                            Gender = 1,
                            Name = "Nguyễn Minh Thư",
                            Phone = "0888901234"
                        },
                        new
                        {
                            Id = 4,
                            Address = "987 Đường Võ Văn Kiệt, Phường 9, Quận 2, Thành phố Hồ Chí Minh",
                            DateOfBirth = new DateTime(1992, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emailtest456@gmail.com",
                            Gender = 0,
                            Name = "Bùi Hoàng Tuấn",
                            Phone = "0899012345"
                        },
                        new
                        {
                            Id = 5,
                            Address = "210 Đường Nguyễn Đình Chính, Phường 10, Quận Tân Phú, Thành phố Hồ Chí Minh",
                            DateOfBirth = new DateTime(1987, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user7890@gmail.com",
                            Gender = 1,
                            Name = "Đỗ Hoài Lan",
                            Phone = "0981122334"
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Detail")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("PromotionId")
                        .HasColumnType("int");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.ToTable("Package", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 1",
                            Price = 1000000.0,
                            Venue = "123 Đường Nguyễn Huệ, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 2,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 2",
                            Price = 1000000.0,
                            Venue = "456 Đường Thảo Điền, Phường Thảo Điền, Quận 2, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 3,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 3",
                            Price = 1000000.0,
                            Venue = "789 Đường Nguyễn Văn Linh, Phường Phạm Ngũ Lão Quận 3 Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 4,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 4",
                            Price = 1000000.0,
                            Venue = "234 Đường Tôn Thất Thuyết, Phường Tân Dinh Quận 4 Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 5,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 5",
                            Price = 1000000.0,
                            Venue = "567 Đường Nguyễn Trãi, Phường 1 Quận 5 Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 6,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 6",
                            Price = 1000000.0,
                            Venue = "890 Đường Huỳnh Văn Bánh, Phường 6, Quận 6, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 7,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 7",
                            Price = 1000000.0,
                            Venue = "321 Đường Nguyễn Thị Minh Khai, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 8,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 8",
                            Price = 1000000.0,
                            Venue = "654 Đường CMT8, Phường 10, Quận 3, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 9,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 9",
                            Price = 1000000.0,
                            Venue = "987 Đường Quang Trung, Phường 7, Quận 9, Thành phố Hồ Chí Minh"
                        },
                        new
                        {
                            Id = 10,
                            Detail = "Trang Trí, Menu Món Ăn, Chương Trình Tiệc",
                            Name = "Gói tiệc sinh nhật quận 12",
                            Price = 1000000.0,
                            Venue = "210 Đường Lê Văn Việt, Phường Thạnh Lộc, Quận 12, Thành phố Hồ Chí Minh"
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageService", b =>
                {
                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("PackageId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("PackageService", (string)null);

                    b.HasData(
                        new
                        {
                            PackageId = 1,
                            ServiceId = 1
                        },
                        new
                        {
                            PackageId = 2,
                            ServiceId = 2
                        },
                        new
                        {
                            PackageId = 3,
                            ServiceId = 3
                        });
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

                    b.Property<int>("Types")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Payment", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1000000.0,
                            BookingId = 1,
                            Date = new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Types = 0
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("DiscountPercent")
                        .HasColumnType("float");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Promotion", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DiscountPercent = 0.10000000000000001,
                            FromDate = new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            HostId = 1,
                            Name = "Sale 10%",
                            Status = 0,
                            ToDate = new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Detail")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Service", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Detail = "Khai Vị, Món Chính, Tráng Miệng",
                            Name = "Menu 3 Món",
                            Price = 1000000.0
                        },
                        new
                        {
                            Id = 2,
                            Detail = "Khai Vị, 2 Món Chính, Tráng Miệng",
                            Name = "Menu 4 Món",
                            Price = 1500000.0
                        },
                        new
                        {
                            Id = 3,
                            Detail = "2 Khai Vị, 2 Món Chính, Tráng Miệng",
                            Name = "Menu 5 Món",
                            Price = 2000000.0
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            Password = "admin123",
                            Role = 0,
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "example123@gmail.com",
                            Password = "123456",
                            Role = 1,
                            Username = "abc"
                        },
                        new
                        {
                            Id = 3,
                            Email = "demo5678@gmail.com",
                            Password = "123456",
                            Role = 2,
                            Username = "toan"
                        });
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Bill", "Bill")
                        .WithOne("Booking")
                        .HasForeignKey("BookingBirthday.Data.Entities.Booking", "BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Host", "Host")
                        .WithMany("Bookings")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Payment", "Payment")
                        .WithOne("Booking")
                        .HasForeignKey("BookingBirthday.Data.Entities.Booking", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("Guest");

                    b.Navigation("Host");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.BookingPackage", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Booking", "Booking")
                        .WithMany("BookingPackages")
                        .HasForeignKey("BookingId")
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Package", "Package")
                        .WithMany("BookingPackages")
                        .HasForeignKey("PackageId")
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.BookingService", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Booking", "Booking")
                        .WithMany("BookingServices")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Service", "Service")
                        .WithMany("BookingServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Cart", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Guest", "Guest")
                        .WithOne("Cart")
                        .HasForeignKey("BookingBirthday.Data.Entities.Cart", "GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.CartPackage", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Cart", "Cart")
                        .WithMany("CartPackages")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Package", "Package")
                        .WithMany("CartPackages")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.CartService", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Cart", "Cart")
                        .WithMany("CartServices")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Service", "Service")
                        .WithMany("CartServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Promotion", "Promotion")
                        .WithMany("Packages")
                        .HasForeignKey("PromotionId");

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.PackageService", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Package", "Package")
                        .WithMany("PackageServices")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBirthday.Data.Entities.Service", "Service")
                        .WithMany("PackageServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Package");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Promotion", b =>
                {
                    b.HasOne("BookingBirthday.Data.Entities.Host", "Host")
                        .WithMany("Promotions")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Host");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Bill", b =>
                {
                    b.Navigation("Booking")
                        .IsRequired();
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Booking", b =>
                {
                    b.Navigation("BookingPackages");

                    b.Navigation("BookingServices");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Cart", b =>
                {
                    b.Navigation("CartPackages");

                    b.Navigation("CartServices");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Guest", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Cart")
                        .IsRequired();
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Host", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Promotions");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Package", b =>
                {
                    b.Navigation("BookingPackages");

                    b.Navigation("CartPackages");

                    b.Navigation("PackageServices");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Payment", b =>
                {
                    b.Navigation("Booking")
                        .IsRequired();
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Promotion", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("BookingBirthday.Data.Entities.Service", b =>
                {
                    b.Navigation("BookingServices");

                    b.Navigation("CartServices");

                    b.Navigation("PackageServices");
                });
#pragma warning restore 612, 618
        }
    }
}
