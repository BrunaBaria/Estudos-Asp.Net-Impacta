using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class allow_wbs_null_for_time_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WBSId",
                table: "TimeRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "TimeRecord_AppointedTime",
                table: "TimeRecord",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WBSId",
                table: "TimeRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TimeRecord_AppointedTime",
                table: "TimeRecord",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
