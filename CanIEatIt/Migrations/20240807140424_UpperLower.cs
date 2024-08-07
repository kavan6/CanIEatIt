using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanIEatIt.Migrations
{
    public partial class UpperLower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageHeight",
                table: "Mushroom",
                newName: "UpperHeight");

            migrationBuilder.RenameColumn(
                name: "AverageDiameter",
                table: "Mushroom",
                newName: "UpperDiameter");

            migrationBuilder.AddColumn<int>(
                name: "LowerDiameter",
                table: "Mushroom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowerHeight",
                table: "Mushroom",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowerDiameter",
                table: "Mushroom");

            migrationBuilder.DropColumn(
                name: "LowerHeight",
                table: "Mushroom");

            migrationBuilder.RenameColumn(
                name: "UpperHeight",
                table: "Mushroom",
                newName: "AverageHeight");

            migrationBuilder.RenameColumn(
                name: "UpperDiameter",
                table: "Mushroom",
                newName: "AverageDiameter");
        }
    }
}
