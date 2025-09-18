using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_wbs_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Hiring_Regime_Work_Schedule",
                table: "HiringRegime",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "WBS",
                columns: table => new
                {
                    WBS_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WBS_Charge_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WBS_Description = table.Column<string>(type: "varchar(10)", nullable: false),
                    WBSTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS", x => x.WBS_Id);
                    table.ForeignKey(
                        name: "FK_WBS_WBSType_WBSTypeId",
                        column: x => x.WBSTypeId,
                        principalTable: "WBSType",
                        principalColumn: "WBS_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WBS_WBSTypeId",
                table: "WBS",
                column: "WBSTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WBS");

            migrationBuilder.AlterColumn<decimal>(
                name: "Hiring_Regime_Work_Schedule",
                table: "HiringRegime",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }
    }
}
