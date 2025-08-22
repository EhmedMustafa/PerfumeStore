using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerfumeStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productvarnatconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
