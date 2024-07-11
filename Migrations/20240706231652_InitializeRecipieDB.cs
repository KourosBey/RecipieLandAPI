using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipieLandAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitializeRecipieDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nutritions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NutritionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NutritionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutritions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipieCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipieCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipies_RecipieCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "RecipieCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowings",
                columns: table => new
                {
                    OwnUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowings", x => new { x.OwnUserId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_UserFollowings_Users_OwnUserId",
                        column: x => x.OwnUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikedCategories",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedCategories", x => new { x.UserId, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_UserLikedCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRecipies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRecipies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipieNutritions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NutritionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipieNutritions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipieNutritions_Nutritions_NutritionId",
                        column: x => x.NutritionId,
                        principalTable: "Nutritions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipieNutritions_Recipies_RecipieId",
                        column: x => x.RecipieId,
                        principalTable: "Recipies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipieSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    RecipieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Carb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protein = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipieSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipieSteps_Recipies_RecipieId",
                        column: x => x.RecipieId,
                        principalTable: "Recipies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikedRecipies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedRecipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikedRecipies_Recipies_RecipieId",
                        column: x => x.RecipieId,
                        principalTable: "Recipies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikedRecipies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipieNutritions_NutritionId",
                table: "RecipieNutritions",
                column: "NutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipieNutritions_RecipieId",
                table: "RecipieNutritions",
                column: "RecipieId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipies_CategoryID",
                table: "Recipies",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipieSteps_RecipieId",
                table: "RecipieSteps",
                column: "RecipieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedRecipies_RecipieId",
                table: "UserLikedRecipies",
                column: "RecipieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedRecipies_UserId",
                table: "UserLikedRecipies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipies_UserId",
                table: "UserRecipies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipieNutritions");

            migrationBuilder.DropTable(
                name: "RecipieSteps");

            migrationBuilder.DropTable(
                name: "UserFollowings");

            migrationBuilder.DropTable(
                name: "UserLikedCategories");

            migrationBuilder.DropTable(
                name: "UserLikedRecipies");

            migrationBuilder.DropTable(
                name: "UserRecipies");

            migrationBuilder.DropTable(
                name: "Nutritions");

            migrationBuilder.DropTable(
                name: "Recipies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RecipieCategories");
        }
    }
}
