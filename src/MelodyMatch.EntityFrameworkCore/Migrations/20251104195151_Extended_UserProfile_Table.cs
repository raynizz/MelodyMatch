using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodyMatch.Migrations
{
    /// <inheritdoc />
    public partial class Extended_UserProfile_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "Interests",
                schema: "public",
                table: "UserProfiles",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "PreferredGenders",
                schema: "public",
                table: "UserProfiles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int>(
                name: "PreferredMaxAge",
                schema: "public",
                table: "UserProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreferredMinAge",
                schema: "public",
                table: "UserProfiles",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interests",
                schema: "public",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreferredGenders",
                schema: "public",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreferredMaxAge",
                schema: "public",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreferredMinAge",
                schema: "public",
                table: "UserProfiles");
        }
    }
}
