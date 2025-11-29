using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFindingsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Findings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Severidad = table.Column<int>(type: "int", nullable: false),
                    FechaHallazgo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Findings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Findings_Audits_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AreaAuditada",
                table: "Audits",
                column: "AreaAuditada");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Estado",
                table: "Audits",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_AuditId",
                table: "Findings",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_Severidad",
                table: "Findings",
                column: "Severidad");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_Tipo",
                table: "Findings",
                column: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Findings");

            migrationBuilder.DropIndex(
                name: "IX_Audits_AreaAuditada",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Estado",
                table: "Audits");
        }
    }
}
