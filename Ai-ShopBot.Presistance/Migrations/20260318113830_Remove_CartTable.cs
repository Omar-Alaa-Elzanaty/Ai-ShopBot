using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ai_ShopBot.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class Remove_CartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.ClientId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Carts_Accounts_ClientId1",
                        column: x => x.ClientId1,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ClientId1",
                table: "Carts",
                column: "ClientId1");
        }
    }
}
