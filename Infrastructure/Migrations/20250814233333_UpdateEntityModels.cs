using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Users_SellerId1",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_SellerId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "SellerId1",
                table: "Bids");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_SellerId",
                table: "Bids",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_SellerId",
                table: "Bids",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Users_SellerId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_SellerId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Bids",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "SellerId1",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_SellerId1",
                table: "Bids",
                column: "SellerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_SellerId1",
                table: "Bids",
                column: "SellerId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
