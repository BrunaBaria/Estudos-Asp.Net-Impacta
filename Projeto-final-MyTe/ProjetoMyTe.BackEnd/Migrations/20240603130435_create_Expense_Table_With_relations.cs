using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class create_Expense_Table_With_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Expense_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Expense_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Expense_Value = table.Column<double>(type: "float", nullable: false),
                    Expense_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WBSId = table.Column<int>(type: "int", nullable: false),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Expense_Id);
                    table.ForeignKey(
                        name: "FK_Expense_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_ExpenseType_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseType",
                        principalColumn: "ExpenseType_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_WBS_WBSId",
                        column: x => x.WBSId,
                        principalTable: "WBS",
                        principalColumn: "WBS_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseTypeId",
                table: "Expense",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_UserId",
                table: "Expense",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_WBSId",
                table: "Expense",
                column: "WBSId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");
        }
    }
}
