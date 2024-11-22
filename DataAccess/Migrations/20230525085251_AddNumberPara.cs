using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityStudyPlatform.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberPara : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberPara",
                table: "Shedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberPara",
                table: "Shedule");
        }
    }
}
