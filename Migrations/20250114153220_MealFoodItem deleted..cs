using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class MealFoodItemdeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealFoodItem");

            migrationBuilder.AddColumn<int>(
                name: "MealId",
                table: "FoodItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_MealId",
                table: "FoodItem",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItem_Meal_MealId",
                table: "FoodItem",
                column: "MealId",
                principalTable: "Meal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItem_Meal_MealId",
                table: "FoodItem");

            migrationBuilder.DropIndex(
                name: "IX_FoodItem_MealId",
                table: "FoodItem");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "FoodItem");

            migrationBuilder.CreateTable(
                name: "MealFoodItem",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "integer", nullable: false),
                    FoodItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealFoodItem", x => new { x.MealId, x.FoodItemId });
                    table.ForeignKey(
                        name: "FK_MealFoodItem_FoodItem_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealFoodItem_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealFoodItem_FoodItemId",
                table: "MealFoodItem",
                column: "FoodItemId");
        }
    }
}
