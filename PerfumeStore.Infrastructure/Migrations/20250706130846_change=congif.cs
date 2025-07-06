using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerfumeStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changecongif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_FragranceNote_ProductId",
                table: "ProductNotes");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_FragranceNoteId",
                table: "ProductNotes",
                column: "FragranceNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_FragranceNote_FragranceNoteId",
                table: "ProductNotes",
                column: "FragranceNoteId",
                principalTable: "FragranceNote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_FragranceNote_FragranceNoteId",
                table: "ProductNotes");

            migrationBuilder.DropIndex(
                name: "IX_ProductNotes_FragranceNoteId",
                table: "ProductNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_FragranceNote_ProductId",
                table: "ProductNotes",
                column: "ProductId",
                principalTable: "FragranceNote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
