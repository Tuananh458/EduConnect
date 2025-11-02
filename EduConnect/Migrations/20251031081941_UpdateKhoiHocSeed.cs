using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKhoiHocSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_maGiaoVienChuNhiem",
                table: "LOPHOC");

            migrationBuilder.DropColumn(
                name: "moTa",
                table: "KHOIHOC");

            migrationBuilder.DropColumn(
                name: "ngayTao",
                table: "KHOIHOC");

            migrationBuilder.DropColumn(
                name: "trangThai",
                table: "KHOIHOC");

            migrationBuilder.RenameColumn(
                name: "maGiaoVienChuNhiem",
                table: "LOPHOC",
                newName: "KhoiHocMaKhoiHoc");

            migrationBuilder.RenameIndex(
                name: "IX_LOPHOC_maGiaoVienChuNhiem",
                table: "LOPHOC",
                newName: "IX_LOPHOC_KhoiHocMaKhoiHoc");

            migrationBuilder.AlterColumn<string>(
                name: "trangThai",
                table: "LOPHOC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "tenLopHoc",
                table: "LOPHOC",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "siSo",
                table: "LOPHOC",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayTao",
                table: "LOPHOC",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "GiaoVienMaGiaoVien",
                table: "LOPHOC",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "KHOIHOC",
                columns: new[] { "maKhoiHoc", "tenKhoiHoc" },
                values: new object[,]
                {
                    { 10, "Khối 10" },
                    { 11, "Khối 11" },
                    { 12, "Khối 12" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_GiaoVienMaGiaoVien",
                table: "LOPHOC",
                column: "GiaoVienMaGiaoVien");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_GiaoVienMaGiaoVien",
                table: "LOPHOC",
                column: "GiaoVienMaGiaoVien",
                principalTable: "GIAOVIEN",
                principalColumn: "maGiaoVien");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                column: "KhoiHocMaKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_GiaoVienMaGiaoVien",
                table: "LOPHOC");

            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC");

            migrationBuilder.DropIndex(
                name: "IX_LOPHOC_GiaoVienMaGiaoVien",
                table: "LOPHOC");

            migrationBuilder.DeleteData(
                table: "KHOIHOC",
                keyColumn: "maKhoiHoc",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "KHOIHOC",
                keyColumn: "maKhoiHoc",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "KHOIHOC",
                keyColumn: "maKhoiHoc",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "GiaoVienMaGiaoVien",
                table: "LOPHOC");

            migrationBuilder.RenameColumn(
                name: "KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                newName: "maGiaoVienChuNhiem");

            migrationBuilder.RenameIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                newName: "IX_LOPHOC_maGiaoVienChuNhiem");

            migrationBuilder.AlterColumn<string>(
                name: "trangThai",
                table: "LOPHOC",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tenLopHoc",
                table: "LOPHOC",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "siSo",
                table: "LOPHOC",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayTao",
                table: "LOPHOC",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "KHOIHOC",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ngayTao",
                table: "KHOIHOC",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "trangThai",
                table: "KHOIHOC",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_maGiaoVienChuNhiem",
                table: "LOPHOC",
                column: "maGiaoVienChuNhiem",
                principalTable: "GIAOVIEN",
                principalColumn: "maGiaoVien",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
