using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class fixlophoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropColumn(
                name: "KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.AlterColumn<Guid>(
                name: "nguoiTaoId",
                table: "LOPHOC",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_nguoiTaoId",
                table: "LOPHOC",
                column: "nguoiTaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_NGUOIDUNG_nguoiTaoId",
                table: "LOPHOC",
                column: "nguoiTaoId",
                principalTable: "NGUOIDUNG",
                principalColumn: "maNguoiDung",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_NGUOIDUNG_nguoiTaoId",
                table: "LOPHOC");

            migrationBuilder.DropIndex(
                name: "IX_LOPHOC_nguoiTaoId",
                table: "LOPHOC");

            migrationBuilder.AlterColumn<Guid>(
                name: "nguoiTaoId",
                table: "LOPHOC",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
        }
    }
}
