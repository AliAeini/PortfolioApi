using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRowVersionFromProjectChildren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectImage_Projects_ProjectId",
                table: "ProjectImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectImage",
                table: "ProjectImage");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProjectImage");

            migrationBuilder.RenameTable(
                name: "ProjectImage",
                newName: "ProjectImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectImage_ProjectId",
                table: "ProjectImages",
                newName: "IX_ProjectImages_ProjectId");

            migrationBuilder.AlterColumn<long>(
                name: "RowId",
                table: "ProjectImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "RevSeq",
                table: "ProjectImages",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCover",
                table: "ProjectImages",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProjectImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ProjectImages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "ProjectImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectImages",
                table: "ProjectImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImages_IsCover",
                table: "ProjectImages",
                column: "IsCover");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectImages_Projects_ProjectId",
                table: "ProjectImages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectImages_Projects_ProjectId",
                table: "ProjectImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectImages",
                table: "ProjectImages");

            migrationBuilder.DropIndex(
                name: "IX_ProjectImages_IsCover",
                table: "ProjectImages");

            migrationBuilder.RenameTable(
                name: "ProjectImages",
                newName: "ProjectImage");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectImages_ProjectId",
                table: "ProjectImage",
                newName: "IX_ProjectImage_ProjectId");

            migrationBuilder.AlterColumn<long>(
                name: "RowId",
                table: "ProjectImage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "RevSeq",
                table: "ProjectImage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCover",
                table: "ProjectImage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProjectImage",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ProjectImage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "ProjectImage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RowVersion",
                table: "ProjectImage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectImage",
                table: "ProjectImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectImage_Projects_ProjectId",
                table: "ProjectImage",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
