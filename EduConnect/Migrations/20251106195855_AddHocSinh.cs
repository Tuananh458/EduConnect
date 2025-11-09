using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddHocSinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "diaChi",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "gioiTinh",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "maNguoiDung",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "trangThai",
                table: "HOCSINH");

            migrationBuilder.RenameColumn(
                name: "ngayNhapHoc",
                table: "HOCSINH",
                newName: "ngayTao");

            migrationBuilder.AddColumn<bool>(
                name: "laLopTruong",
                table: "HOCSINH",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "maDinhDanh",
                table: "HOCSINH",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "userId",
                table: "HOCSINH",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_userId",
                table: "HOCSINH",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_HOCSINH_Users_userId",
                table: "HOCSINH",
                column: "userId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HOCSINH_Users_userId",
                table: "HOCSINH");

            migrationBuilder.DropIndex(
                name: "IX_HOCSINH_userId",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "laLopTruong",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "maDinhDanh",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "HOCSINH");

            migrationBuilder.RenameColumn(
                name: "ngayTao",
                table: "HOCSINH",
                newName: "ngayNhapHoc");

            migrationBuilder.AddColumn<string>(
                name: "diaChi",
                table: "HOCSINH",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "gioiTinh",
                table: "HOCSINH",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "maNguoiDung",
                table: "HOCSINH",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "trangThai",
                table: "HOCSINH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
