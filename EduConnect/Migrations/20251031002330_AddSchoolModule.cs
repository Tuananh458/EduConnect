using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GIAOVIEN",
                columns: table => new
                {
                    maGiaoVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    chuyenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    trinhDo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ngayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    trangThaiCongTac = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIAOVIEN", x => x.maGiaoVien);
                });

            migrationBuilder.CreateTable(
                name: "KHOIHOC",
                columns: table => new
                {
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenKhoiHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHOIHOC", x => x.maKhoiHoc);
                });

            migrationBuilder.CreateTable(
                name: "PHUHUYNH",
                columns: table => new
                {
                    maPhuHuynh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    ngheNghiep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mucDoLienKet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHUHUYNH", x => x.maPhuHuynh);
                });

            migrationBuilder.CreateTable(
                name: "LOPHOC",
                columns: table => new
                {
                    maLopHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenLopHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false),
                    maGiaoVienChuNhiem = table.Column<int>(type: "int", nullable: true),
                    siSo = table.Column<int>(type: "int", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOPHOC", x => x.maLopHoc);
                    table.ForeignKey(
                        name: "FK_LOPHOC_GIAOVIEN_maGiaoVienChuNhiem",
                        column: x => x.maGiaoVienChuNhiem,
                        principalTable: "GIAOVIEN",
                        principalColumn: "maGiaoVien",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                        column: x => x.maKhoiHoc,
                        principalTable: "KHOIHOC",
                        principalColumn: "maKhoiHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    maHocSinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    maLopHoc = table.Column<int>(type: "int", nullable: false),
                    ngaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ngayNhapHoc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCSINH", x => x.maHocSinh);
                    table.ForeignKey(
                        name: "FK_HOCSINH_LOPHOC_maLopHoc",
                        column: x => x.maLopHoc,
                        principalTable: "LOPHOC",
                        principalColumn: "maLopHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIENKET_PHUHUYNH_HOCSINH",
                columns: table => new
                {
                    maLienKet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhuHuynh = table.Column<int>(type: "int", nullable: false),
                    maHocSinh = table.Column<int>(type: "int", nullable: false),
                    moiQuanHe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    trangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIENKET_PHUHUYNH_HOCSINH", x => x.maLienKet);
                    table.ForeignKey(
                        name: "FK_LIENKET_PHUHUYNH_HOCSINH_HOCSINH_maHocSinh",
                        column: x => x.maHocSinh,
                        principalTable: "HOCSINH",
                        principalColumn: "maHocSinh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LIENKET_PHUHUYNH_HOCSINH_PHUHUYNH_maPhuHuynh",
                        column: x => x.maPhuHuynh,
                        principalTable: "PHUHUYNH",
                        principalColumn: "maPhuHuynh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_maLopHoc",
                table: "HOCSINH",
                column: "maLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maHocSinh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maHocSinh");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maPhuHuynh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maPhuHuynh");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_maGiaoVienChuNhiem",
                table: "LOPHOC",
                column: "maGiaoVienChuNhiem");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_maKhoiHoc",
                table: "LOPHOC",
                column: "maKhoiHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LIENKET_PHUHUYNH_HOCSINH");

            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "PHUHUYNH");

            migrationBuilder.DropTable(
                name: "LOPHOC");

            migrationBuilder.DropTable(
                name: "GIAOVIEN");

            migrationBuilder.DropTable(
                name: "KHOIHOC");
        }
    }
}
