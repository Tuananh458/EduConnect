using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class addnguoitaoidtohoclieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "nguoiTaoId",
                table: "HOCLIEU",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HOCLIEU_nguoiTaoId",
                table: "HOCLIEU",
                column: "nguoiTaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_HOCLIEU_NGUOIDUNG_nguoiTaoId",
                table: "HOCLIEU",
                column: "nguoiTaoId",
                principalTable: "NGUOIDUNG",
                principalColumn: "maNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HOCLIEU_NGUOIDUNG_nguoiTaoId",
                table: "HOCLIEU");

            migrationBuilder.DropIndex(
                name: "IX_HOCLIEU_nguoiTaoId",
                table: "HOCLIEU");

            migrationBuilder.DropColumn(
                name: "nguoiTaoId",
                table: "HOCLIEU");
        }
    }
}
