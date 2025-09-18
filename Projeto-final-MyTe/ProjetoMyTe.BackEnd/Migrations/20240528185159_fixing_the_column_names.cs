using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class fixing_the_column_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Department",
                newName: "Department_Name");

            migrationBuilder.RenameColumn(
                name: "ContactEmail",
                table: "Department",
                newName: "Department_Contact_Email");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Department",
                newName: "Department_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Department_Name",
                table: "Department",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Department_Contact_Email",
                table: "Department",
                newName: "ContactEmail");

            migrationBuilder.RenameColumn(
                name: "Department_Id",
                table: "Department",
                newName: "DepartmentId");
        }
    }
}
