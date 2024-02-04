using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBirthday.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Host",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Host", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Types = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<double>(type: "float", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Guest_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountPercent = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Host_HostId",
                        column: x => x.HostId,
                        principalTable: "Host",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Total = table.Column<double>(type: "float", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Guest_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Host_HostId",
                        column: x => x.HostId,
                        principalTable: "Host",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartService",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartService", x => new { x.CartId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_CartService_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingService",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingService", x => new { x.BookingId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_BookingService_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPackage",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPackage", x => new { x.BookingId, x.PackageId });
                    table.ForeignKey(
                        name: "FK_BookingPackage_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingPackage_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartPackage",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartPackage", x => new { x.CartId, x.PackageId });
                    table.ForeignKey(
                        name: "FK_CartPackage_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartPackage_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageService",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageService", x => new { x.PackageId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_PackageService_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bill",
                columns: new[] { "Id", "BookingId", "Date", "Discount", "Total" },
                values: new object[] { 1, 1, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, 0.0 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "Id", "Address", "CartId", "DateOfBirth", "Email", "Name", "Phone" },
                values: new object[] { 1, "123 Đường Nguyễn Văn Linh, Phường 1, Quận 10, Thành phố Hồ Chí Minh", 1, new DateTime(1989, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "example123@gmail.com", "Nguyễn Tấn Đạt", "0961234567" });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "Id", "Address", "CartId", "DateOfBirth", "Email", "Gender", "Name", "Phone" },
                values: new object[] { 2, "456 Đường Lê Lợi, Phường 2, Quận Bình Thạnh, Thành phố Hồ Chí Minh", 2, new DateTime(1995, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4567@gmail.com", 1, "Trần Thị Ngọc Mai", "0902345678" });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "Id", "Address", "CartId", "DateOfBirth", "Email", "Name", "Phone" },
                values: new object[] { 3, "789 Đường Trần Hưng Đạo, Phường 3, Quận 5, Thành phố Hồ Chí Minh", 3, new DateTime(1983, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "testing789@gmail.com", "Lê Văn Hiếu", "0933456789" });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "Id", "Address", "CartId", "DateOfBirth", "Email", "Gender", "Name", "Phone" },
                values: new object[,]
                {
                    { 4, "234 Đường Cách Mạng Tháng Tám, Phường 4, Quận 3, Thành phố Hồ Chí Minh", 4, new DateTime(1990, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "randomuser12@gmail.com", 1, "Phạm Tuyết Hoa", "0914567890" },
                    { 5, "567 Đường Hoàng Hoa Thám, Phường 5, Quận 6, Thành phố Hồ Chí Minh", 5, new DateTime(1976, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "email34@gmail.com", 1, "Nguyễn Thị Anh", "0975678901" }
                });

            migrationBuilder.InsertData(
                table: "Host",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "890 Đường Nguyễn Công Trứ, Phường 6, Quận 1, Thành phố Hồ Chí Minh", new DateTime(1982, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo5678@gmail.com", "Võ Quốc Anh", "0926789012" },
                    { 2, "321 Đường Lý Thường Kiệt, Phường 7, Quận Tân Bình, Thành phố Hồ Chí Minh", new DateTime(1998, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "username901@gmail.com", "Hoàng Văn Quân", "0947890123" }
                });

            migrationBuilder.InsertData(
                table: "Host",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "Gender", "Name", "Phone" },
                values: new object[] { 3, "654 Đường Trần Phú, Phường 8, Quận Gò Vấp, Thành phố Hồ Chí Minh", new DateTime(1974, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "sample23@gmail.com", 1, "Nguyễn Minh Thư", "0888901234" });

            migrationBuilder.InsertData(
                table: "Host",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "Name", "Phone" },
                values: new object[] { 4, "987 Đường Võ Văn Kiệt, Phường 9, Quận 2, Thành phố Hồ Chí Minh", new DateTime(1992, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "emailtest456@gmail.com", "Bùi Hoàng Tuấn", "0899012345" });

            migrationBuilder.InsertData(
                table: "Host",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "Gender", "Name", "Phone" },
                values: new object[] { 5, "210 Đường Nguyễn Đình Chính, Phường 10, Quận Tân Phú, Thành phố Hồ Chí Minh", new DateTime(1987, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "user7890@gmail.com", 1, "Đỗ Hoài Lan", "0981122334" });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "Detail", "Name", "Price", "PromotionId", "Venue" },
                values: new object[,]
                {
                    { 1, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 1", 1000000.0, null, "123 Đường Nguyễn Huệ, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh" },
                    { 2, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 2", 1000000.0, null, "456 Đường Thảo Điền, Phường Thảo Điền, Quận 2, Thành phố Hồ Chí Minh" },
                    { 3, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 3", 1000000.0, null, "789 Đường Nguyễn Văn Linh, Phường Phạm Ngũ Lão Quận 3 Thành phố Hồ Chí Minh" },
                    { 4, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 4", 1000000.0, null, "234 Đường Tôn Thất Thuyết, Phường Tân Dinh Quận 4 Thành phố Hồ Chí Minh" },
                    { 5, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 5", 1000000.0, null, "567 Đường Nguyễn Trãi, Phường 1 Quận 5 Thành phố Hồ Chí Minh" },
                    { 6, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 6", 1000000.0, null, "890 Đường Huỳnh Văn Bánh, Phường 6, Quận 6, Thành phố Hồ Chí Minh" },
                    { 7, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 7", 1000000.0, null, "321 Đường Nguyễn Thị Minh Khai, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh" },
                    { 8, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 8", 1000000.0, null, "654 Đường CMT8, Phường 10, Quận 3, Thành phố Hồ Chí Minh" },
                    { 9, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 9", 1000000.0, null, "987 Đường Quang Trung, Phường 7, Quận 9, Thành phố Hồ Chí Minh" },
                    { 10, "Trang Trí, Menu Món Ăn, Chương Trình Tiệc", "Gói tiệc sinh nhật quận 12", 1000000.0, null, "210 Đường Lê Văn Việt, Phường Thạnh Lộc, Quận 12, Thành phố Hồ Chí Minh" }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "Amount", "BookingId", "Date", "Types" },
                values: new object[] { 1, 1000000.0, 1, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Detail", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Khai Vị, Món Chính, Tráng Miệng", "Menu 3 Món", 1000000.0 },
                    { 2, "Khai Vị, 2 Món Chính, Tráng Miệng", "Menu 4 Món", 1500000.0 },
                    { 3, "2 Khai Vị, 2 Món Chính, Tráng Miệng", "Menu 5 Món", 2000000.0 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", "admin123", 0, "admin" },
                    { 2, "example123@gmail.com", "123456", 1, "abc" },
                    { 3, "demo5678@gmail.com", "123456", 2, "toan" }
                });

            migrationBuilder.InsertData(
                table: "Booking",
                columns: new[] { "Id", "BillId", "Date", "GuestId", "HostId", "PaymentId", "Total" },
                values: new object[] { 1, 1, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, 0.0 });

            migrationBuilder.InsertData(
                table: "Cart",
                columns: new[] { "Id", "GuestId", "Total" },
                values: new object[,]
                {
                    { 1, 1, 0.0 },
                    { 2, 2, 0.0 },
                    { 3, 3, 0.0 },
                    { 4, 4, 0.0 },
                    { 5, 5, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "PackageService",
                columns: new[] { "PackageId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Promotion",
                columns: new[] { "Id", "DiscountPercent", "FromDate", "HostId", "Name", "Status", "ToDate" },
                values: new object[] { 1, 0.10000000000000001, new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Sale 10%", 0, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "BookingPackage",
                columns: new[] { "BookingId", "PackageId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "BookingService",
                columns: new[] { "BookingId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "CartPackage",
                columns: new[] { "CartId", "PackageId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "CartService",
                columns: new[] { "CartId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BookingId",
                table: "Bill",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BillId",
                table: "Booking",
                column: "BillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_GuestId",
                table: "Booking",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_HostId",
                table: "Booking",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPackage_PackageId",
                table: "BookingPackage",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingService_ServiceId",
                table: "BookingService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_GuestId",
                table: "Cart",
                column: "GuestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartPackage_PackageId",
                table: "CartPackage",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_CartService_ServiceId",
                table: "CartService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_CartId",
                table: "Guest",
                column: "CartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Package_PromotionId",
                table: "Package",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageService_ServiceId",
                table: "PackageService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_HostId",
                table: "Promotion",
                column: "HostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPackage");

            migrationBuilder.DropTable(
                name: "BookingService");

            migrationBuilder.DropTable(
                name: "CartPackage");

            migrationBuilder.DropTable(
                name: "CartService");

            migrationBuilder.DropTable(
                name: "PackageService");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Host");
        }
    }
}
