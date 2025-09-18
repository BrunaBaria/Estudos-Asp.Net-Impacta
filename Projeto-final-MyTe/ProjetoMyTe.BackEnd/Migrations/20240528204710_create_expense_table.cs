using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_expense_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseType",
                columns: table => new
                {
                    ExpenseType_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseType_Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseType", x => x.ExpenseType_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseType");
        }
    }
}
