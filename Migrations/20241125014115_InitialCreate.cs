using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SinqiaParibas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    DES_PRODUTO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    STA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.COD_PRODUTO);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_COSIF",
                columns: table => new
                {
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    COD_COSIF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    COD_CLASSIFICACAO = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    STA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_COSIF", x => new { x.COD_PRODUTO, x.COD_COSIF });
                    table.ForeignKey(
                        name: "FK_PRODUTO_COSIF_PRODUTO_COD_PRODUTO",
                        column: x => x.COD_PRODUTO,
                        principalTable: "PRODUTO",
                        principalColumn: "COD_PRODUTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MOVIMENTO_MANUAL",
                columns: table => new
                {
                    DAT_MES = table.Column<int>(type: "int", nullable: false),
                    DAT_ANO = table.Column<int>(type: "int", nullable: false),
                    NUM_LANCAMENTO = table.Column<int>(type: "int", nullable: false),
                    COD_PRODUTO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    COD_COSIF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DES_DESCRICAO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DAT_MOVIMENTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COD_USUARIO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    VAL_VALOR = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOVIMENTO_MANUAL", x => new { x.DAT_MES, x.DAT_ANO, x.NUM_LANCAMENTO });
                    table.ForeignKey(
                        name: "FK_MOVIMENTO_MANUAL_PRODUTO_COSIF_COD_PRODUTO_COD_COSIF",
                        columns: x => new { x.COD_PRODUTO, x.COD_COSIF },
                        principalTable: "PRODUTO_COSIF",
                        principalColumns: new[] { "COD_PRODUTO", "COD_COSIF" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOVIMENTO_MANUAL_COD_PRODUTO_COD_COSIF",
                table: "MOVIMENTO_MANUAL",
                columns: new[] { "COD_PRODUTO", "COD_COSIF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOVIMENTO_MANUAL");

            migrationBuilder.DropTable(
                name: "PRODUTO_COSIF");

            migrationBuilder.DropTable(
                name: "PRODUTO");
        }
    }
}
