using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Octweet.ConsoleApp.Migrations
{
    public partial class AnnotationContainsTextFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContainsText",
                table: "EntityAnnotations",
                type: "tinyint(1)",
                nullable: true,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainsText",
                table: "EntityAnnotations");
        }
    }
}
