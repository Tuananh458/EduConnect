using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class fixlophoc_fk_relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.AddColumn<int>(
                name: "KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                column: "KhoiHocMaKhoiHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                column: "KhoiHocMaKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                table: "LOPHOC",
                column: "maKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropColumn(
                name: "KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                table: "LOPHOC",
                column: "maKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
