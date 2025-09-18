using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_TimeRecord_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeRecord",
                columns: table => new
                {
                    TimeRecord_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeRecord_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeRecord_AppointedTime = table.Column<double>(type: "float", nullable: false),
                    WBSId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRecord", x => x.TimeRecord_Id);
                    table.ForeignKey(
                        name: "FK_TimeRecord_WBS_WBSId",
                        column: x => x.WBSId,
                        principalTable: "WBS",
                        principalColumn: "WBS_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_WBSId",
                table: "TimeRecord",
                column: "WBSId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeRecord");
        }
    }
}
