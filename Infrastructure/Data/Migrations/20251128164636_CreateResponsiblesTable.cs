using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateResponsiblesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsable",
                table: "Audits");

            migrationBuilder.AddColumn<int>(
                name: "ResponsibleId",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Responsibles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsibles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_ResponsibleId",
                table: "Audits",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibles_Area",
                table: "Responsibles",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibles_Correo",
                table: "Responsibles",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responsibles_Nombre",
                table: "Responsibles",
                column: "Nombre");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Responsibles_ResponsibleId",
                table: "Audits",
                column: "ResponsibleId",
                principalTable: "Responsibles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Responsibles_ResponsibleId",
                table: "Audits");

            migrationBuilder.DropTable(
                name: "Responsibles");

            migrationBuilder.DropIndex(
                name: "IX_Audits_ResponsibleId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ResponsibleId",
                table: "Audits");

            migrationBuilder.AddColumn<string>(
                name: "Responsable",
                table: "Audits",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
