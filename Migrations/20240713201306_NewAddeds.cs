using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipieLandAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewAddeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryImage",
                table: "RecipieCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryImage",
                table: "RecipieCategories");
        }
    }
}
