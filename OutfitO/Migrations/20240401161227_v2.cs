using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutfitO.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
