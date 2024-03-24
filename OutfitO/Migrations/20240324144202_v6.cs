using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutfitO.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PromoCode_PromoCodeId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode");

            migrationBuilder.RenameTable(
                name: "PromoCode",
                newName: "Promo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promo",
                table: "Promo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promo_PromoCodeId",
                table: "Order",
                column: "PromoCodeId",
                principalTable: "Promo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promo_PromoCodeId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promo",
                table: "Promo");

            migrationBuilder.RenameTable(
                name: "Promo",
                newName: "PromoCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PromoCode_PromoCodeId",
                table: "Order",
                column: "PromoCodeId",
                principalTable: "PromoCode",
                principalColumn: "Id");
        }
    }
}
