using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Segfy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apolices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroApolice = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NomeSegurado = table.Column<string>(type: "TEXT", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ramo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apolices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sinistros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroSinistro = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DataSinistro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ValorSolicitado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ValorAprovado = table.Column<decimal>(type: "TEXT", nullable: true),
                    MotivoNegativa = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ApoliceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinistros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sinistros_Apolices_ApoliceId",
                        column: x => x.ApoliceId,
                        principalTable: "Apolices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoSinistros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SinistroId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusAnterior = table.Column<int>(type: "INTEGER", nullable: true),
                    StatusNovo = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoSinistros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoSinistros_Sinistros_SinistroId",
                        column: x => x.SinistroId,
                        principalTable: "Sinistros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apolices_NumeroApolice",
                table: "Apolices",
                column: "NumeroApolice",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSinistros_SinistroId",
                table: "HistoricoSinistros",
                column: "SinistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistros_ApoliceId",
                table: "Sinistros",
                column: "ApoliceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sinistros_NumeroSinistro",
                table: "Sinistros",
                column: "NumeroSinistro",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoSinistros");

            migrationBuilder.DropTable(
                name: "Sinistros");

            migrationBuilder.DropTable(
                name: "Apolices");
        }
    }
}
