using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectProfileRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProfileId",
                table: "Projects",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Profiles_ProfileId",
                table: "Projects",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Profiles_ProfileId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProfileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Projects");
        }
    }
}
