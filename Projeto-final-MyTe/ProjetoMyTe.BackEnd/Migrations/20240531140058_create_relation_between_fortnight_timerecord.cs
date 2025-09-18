using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_relation_between_fortnight_timerecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FortnightId",
                table: "TimeRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_FortnightId",
                table: "TimeRecord",
                column: "FortnightId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRecord_Fortnight_FortnightId",
                table: "TimeRecord",
                column: "FortnightId",
                principalTable: "Fortnight",
                principalColumn: "Fortnight_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeRecord_Fortnight_FortnightId",
                table: "TimeRecord");

            migrationBuilder.DropIndex(
                name: "IX_TimeRecord_FortnightId",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "FortnightId",
                table: "TimeRecord");
        }
    }
}
