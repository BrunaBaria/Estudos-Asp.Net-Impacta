using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class update_AppUser_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppUser_Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppUser_Admission_Date",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HiringRegimeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HiringRegimeId",
                table: "AspNetUsers",
                column: "HiringRegimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LocationId",
                table: "AspNetUsers",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Department_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HiringRegime_HiringRegimeId",
                table: "AspNetUsers",
                column: "HiringRegimeId",
                principalTable: "HiringRegime",
                principalColumn: "Hiring_Regime_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Location_LocationId",
                table: "AspNetUsers",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Location_Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HiringRegime_HiringRegimeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Location_LocationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HiringRegimeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LocationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUser_Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUser_Admission_Date",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HiringRegimeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AspNetUsers");
        }
    }
}
