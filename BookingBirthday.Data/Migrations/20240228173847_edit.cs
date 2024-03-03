using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingBirthday.Data.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Payment_PaymentId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Payment_PaymentId",
                table: "Booking",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Payment_PaymentId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentId",
                table: "Booking",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Payment_PaymentId",
                table: "Booking",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
