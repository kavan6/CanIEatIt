using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanIEatIt.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mushroom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapDiameter = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Edible = table.Column<bool>(type: "bit", nullable: false),
                    EdibleDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GillDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SporeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MicroscopicDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mushroom", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mushroom");
        }
    }
}
