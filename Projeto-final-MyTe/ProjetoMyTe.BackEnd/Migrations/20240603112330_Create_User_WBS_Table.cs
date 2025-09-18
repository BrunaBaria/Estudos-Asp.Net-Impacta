using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeProject.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class Create_User_WBS_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserWBS",
                columns: table => new
                {
                    User_WBS_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WBSId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWBS", x => x.User_WBS_Id);
                    table.ForeignKey(
                        name: "FK_UserWBS_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWBS_WBS_WBSId",
                        column: x => x.WBSId,
                        principalTable: "WBS",
                        principalColumn: "WBS_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWBS_UserId",
                table: "UserWBS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWBS_WBSId",
                table: "UserWBS",
                column: "WBSId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWBS");
        }
    }
}
