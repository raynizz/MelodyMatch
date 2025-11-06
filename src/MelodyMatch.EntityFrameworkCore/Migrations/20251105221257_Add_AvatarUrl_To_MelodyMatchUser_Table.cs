using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodyMatch.Migrations
{
    /// <inheritdoc />
    public partial class Add_AvatarUrl_To_MelodyMatchUser_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "public",
                table: "MelodyMatchUsers",
                type: "text",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "public",
                table: "MelodyMatchUsers");
        }
    }
}
