using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class InitCauHoiDapAnModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_KHOIHOC_maKhoiHoc",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_LOPHOC_maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CHUDE_CHUONG_maChuong",
                table: "CHUDE");

            migrationBuilder.DropForeignKey(
                name: "FK_CHUONG_KHOIHOC_maKhoiHoc",
                table: "CHUONG");

            migrationBuilder.DropIndex(
                name: "IX_CHUONG_maKhoiHoc",
                table: "CHUONG");

            migrationBuilder.DropIndex(
                name: "IX_CHUDE_maChuong",
                table: "CHUDE");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maKhoiHoc",
                table: "CAUHOI");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maKhoiHoc",
                table: "CHUONG");

            migrationBuilder.DropColumn(
                name: "moTa",
                table: "CHUONG");

            migrationBuilder.DropColumn(
                name: "maChuong",
                table: "CHUDE");

            migrationBuilder.DropColumn(
                name: "moTa",
                table: "CHUDE");

            migrationBuilder.DropColumn(
                name: "huongDanGiai",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "loaiCauHoi",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maKhoiHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "monHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "soDapAnDung",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "trangThai",
                table: "CAUHOI");

            migrationBuilder.RenameColumn(
                name: "mucDo",
                table: "CAUHOI",
                newName: "doKho");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "DAPAN",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedDate",
                table: "DAPAN",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI",
                column: "maChuong",
                principalTable: "CHUONG",
                principalColumn: "maChuong",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "DAPAN");

            migrationBuilder.DropColumn(
                name: "updatedDate",
                table: "DAPAN");

            migrationBuilder.RenameColumn(
                name: "doKho",
                table: "CAUHOI",
                newName: "mucDo");

            migrationBuilder.AddColumn<int>(
                name: "maKhoiHoc",
                table: "CHUONG",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "CHUONG",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maChuong",
                table: "CHUDE",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "CHUDE",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "huongDanGiai",
                table: "CAUHOI",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "loaiCauHoi",
                table: "CAUHOI",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "maKhoiHoc",
                table: "CAUHOI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maLopHoc",
                table: "CAUHOI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "monHoc",
                table: "CAUHOI",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "soDapAnDung",
                table: "CAUHOI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "trangThai",
                table: "CAUHOI",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CHUONG_maKhoiHoc",
                table: "CHUONG",
                column: "maKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_CHUDE_maChuong",
                table: "CHUDE",
                column: "maChuong");

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maKhoiHoc",
                table: "CAUHOI",
                column: "maKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maLopHoc",
                table: "CAUHOI",
                column: "maLopHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI",
                column: "maChuong",
                principalTable: "CHUONG",
                principalColumn: "maChuong");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_KHOIHOC_maKhoiHoc",
                table: "CAUHOI",
                column: "maKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_LOPHOC_maLopHoc",
                table: "CAUHOI",
                column: "maLopHoc",
                principalTable: "LOPHOC",
                principalColumn: "maLopHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_CHUDE_CHUONG_maChuong",
                table: "CHUDE",
                column: "maChuong",
                principalTable: "CHUONG",
                principalColumn: "maChuong",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHUONG_KHOIHOC_maKhoiHoc",
                table: "CHUONG",
                column: "maKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc");
        }
    }
}
