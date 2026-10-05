using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class AddJobCategoryAndProfileDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SocialLinks_Platform",
                table: "SocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_Email",
                table: "Profiles");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "SocialLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "AvailableForHire",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Profiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hobbies",
                table: "Profiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobCategoryId",
                table: "Profiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Profiles",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Profiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Profiles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortBio",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Profiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevSeq = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_ProfileId",
                table: "SocialLinks",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Email",
                table: "Profiles",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL AND \"Status\" <> 5");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_JobCategoryId",
                table: "Profiles",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCategories_Name",
                table: "JobCategories",
                column: "Name",
                unique: true,
                filter: "\"Name\" IS NOT NULL AND \"Status\" <> 5");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_JobCategories_JobCategoryId",
                table: "Profiles",
                column: "JobCategoryId",
                principalTable: "JobCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLinks_Profiles_ProfileId",
                table: "SocialLinks",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_JobCategories_JobCategoryId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialLinks_Profiles_ProfileId",
                table: "SocialLinks");

            migrationBuilder.DropTable(
                name: "JobCategories");

            migrationBuilder.DropIndex(
                name: "IX_SocialLinks_ProfileId",
                table: "SocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_Email",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_JobCategoryId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "AvailableForHire",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Hobbies",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "JobCategoryId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ShortBio",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Profiles");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_Platform",
                table: "SocialLinks",
                column: "Platform");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Email",
                table: "Profiles",
                column: "Email");
        }
    }
}
