using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_Hiring_regime_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HiringRegime",
                columns: table => new
                {
                    Hiring_Regime_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hiring_Regime_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hiring_Regime_Accept_Overtime = table.Column<bool>(type: "bit", nullable: false),
                    Hiring_Regime_Work_Schedule = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiringRegime", x => x.Hiring_Regime_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HiringRegime");
        }
    }
}
