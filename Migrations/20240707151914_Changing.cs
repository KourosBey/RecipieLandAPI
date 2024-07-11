using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipieLandAPI.Migrations
{
    /// <inheritdoc />
    public partial class Changing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_Users_OwnUserId",
                table: "UserFollowings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedCategories_Users_UserId",
                table: "UserLikedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedRecipies_Users_UserId",
                table: "UserLikedRecipies");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "RecipieSteps");

            migrationBuilder.DropColumn(
                name: "Carb",
                table: "RecipieSteps");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "RecipieSteps");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "RecipieSteps");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserRecipieId",
                table: "UserRecipies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "value",
                table: "UserLikedRecipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Calories",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Carb",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fat",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreparationTime",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Protein",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Serve",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipies_RecipieId",
                table: "UserRecipies",
                column: "RecipieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipies_UserRecipieId",
                table: "UserRecipies",
                column: "UserRecipieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_Users_OwnUserId",
                table: "UserFollowings",
                column: "OwnUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedCategories_Users_UserId",
                table: "UserLikedCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedRecipies_Users_UserId",
                table: "UserLikedRecipies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipies_Recipies_RecipieId",
                table: "UserRecipies",
                column: "RecipieId",
                principalTable: "Recipies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipies_Users_UserRecipieId",
                table: "UserRecipies",
                column: "UserRecipieId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_Users_OwnUserId",
                table: "UserFollowings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedCategories_Users_UserId",
                table: "UserLikedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedRecipies_Users_UserId",
                table: "UserLikedRecipies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipies_Recipies_RecipieId",
                table: "UserRecipies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipies_Users_UserRecipieId",
                table: "UserRecipies");

            migrationBuilder.DropIndex(
                name: "IX_UserRecipies_RecipieId",
                table: "UserRecipies");

            migrationBuilder.DropIndex(
                name: "IX_UserRecipies_UserRecipieId",
                table: "UserRecipies");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Biography",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserRecipieId",
                table: "UserRecipies");

            migrationBuilder.DropColumn(
                name: "value",
                table: "UserLikedRecipies");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "Carb",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "Recipies");

            migrationBuilder.DropColumn(
                name: "Serve",
                table: "Recipies");

            migrationBuilder.AddColumn<string>(
                name: "Calories",
                table: "RecipieSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Carb",
                table: "RecipieSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fat",
                table: "RecipieSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Protein",
                table: "RecipieSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_Users_OwnUserId",
                table: "UserFollowings",
                column: "OwnUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedCategories_Users_UserId",
                table: "UserLikedCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedRecipies_Users_UserId",
                table: "UserLikedRecipies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
