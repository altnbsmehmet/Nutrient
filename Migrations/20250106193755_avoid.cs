using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class avoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "FoodItem",
                newName: "Portion");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FoodItem",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "FoodItem",
                newName: "Calorie");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FoodItem",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FoodItem");

            migrationBuilder.RenameColumn(
                name: "Portion",
                table: "FoodItem",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FoodItem",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Calorie",
                table: "FoodItem",
                newName: "Calories");
        }
    }
}
