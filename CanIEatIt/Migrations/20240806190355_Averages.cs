using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanIEatIt.Migrations
{
    public partial class Averages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AverageDiameter",
                table: "Mushroom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageHeight",
                table: "Mushroom",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDiameter",
                table: "Mushroom");

            migrationBuilder.DropColumn(
                name: "AverageHeight",
                table: "Mushroom");
        }
    }
}
